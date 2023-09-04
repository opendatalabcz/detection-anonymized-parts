using CommandLine;
using Contracts.Analyzer;
using Contracts.Results;
using Domain.DocumentAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ConsoleApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
              .WithParsed(opts => HandleOptions(opts));
        }
        static void HandleOptions(Options options)
        {
            // init api
            var factory = new WebApplicationFactory<API2.Program>();
            var client = factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");

            var fileLocation = options.FileLocation;
            var returnImages = options.ReturnImages;
            var outputFolder = options.OutputFolder;


            request.Content =
                    new StringContent(
                        JsonConvert.SerializeObject(
                            new AnalyzeDocumentRequest(
                                FileLocation: fileLocation,
                                ReturnImages: returnImages
                            )
                        ),
                        Encoding.UTF8,
                        "application/json"
                    );
            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var parsedJson = JObject.Parse(responseContent);
            Console.WriteLine(parsedJson);
            var documentId = parsedJson["documentId"].ToString();

            request = new HttpRequestMessage(HttpMethod.Get, "/results");
            request.Content =
                new StringContent(
                    JsonConvert.SerializeObject(
                        new GetDocumentPagesRequest(
                            DocumentId: documentId
                            )
                        ),
                        Encoding.UTF8,
                        "application/json"
                    );
            response = client.SendAsync(request).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;
            parsedJson = JObject.Parse(responseContent);
            // save images to output folder
            if (outputFolder != null)
            {
                var pages = parsedJson["pages"];
                for (int i = 1; i <= pages.Count(); i++)
                {
                    var page = pages[i.ToString()];
                    var imageOriginal = page["Original"].ToString();
                    var imageResult = page["Result"].ToString();
                    var imageBytes = Convert.FromBase64String(imageOriginal);
                    var imageFilePath = $"{outputFolder}/original_{i}.jpg";
                    // ensure folder exists
                    Directory.CreateDirectory(Path.GetDirectoryName(imageFilePath));
                    File.WriteAllBytes(imageFilePath, imageBytes);
                    imageBytes = Convert.FromBase64String(imageResult);
                    imageFilePath = $"{outputFolder}/result_{i}.jpg";
                    File.WriteAllBytes(imageFilePath, imageBytes);
                }
            }
        }
    }

    /// <summary>
    /// Options for the console app.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// File location.
        /// </summary>
        [Option("file-location", Required = true, HelpText = "Set the file location.")]
        public string FileLocation { get; set; } = default!;

        /// <summary>
        /// Return images.
        /// </summary>
        [Option("return-images", Default = false, HelpText = "Set to true to return images, defaults to false.")]
        public bool ReturnImages { get; set; } = false;

        /// <summary>
        /// Output folder.
        /// </summary>
        [Option("output-folder", Default = null, HelpText = "Output folder for images, defaults to null.")]
        public string OutputFolder { get; set; } = default!;
    }
}
