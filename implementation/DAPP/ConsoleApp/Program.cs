using CommandLine;
using Contracts.Analyzer;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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


            request.Content = request.Content =
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
            // Act
            var response = client.SendAsync(request).Result;
            Console.WriteLine(response);
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
        [Value(0, Required = true, HelpText = "Set the file location.")]
        public string FileLocation { get; set; } = default!;

        /// <summary>
        /// Return images.
        /// </summary>
        [Value(1, Default = false, HelpText = "Set to true to return images, defaults to false.")]
        public bool ReturnImages { get; set; } = false;

        /// <summary>
        /// Output folder.
        /// </summary>
        [Value(2, Default = null, HelpText = "Output folder for images, defaults to null.")]
        public string OutputFolder { get; set; } = default!;
    }
}
