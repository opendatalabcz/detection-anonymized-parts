using Contracts.Analyzer;
using Contracts.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GenerateTestResults;

public class Program
{
    public static void Main()
    {
        // init api
        var factory = new WebApplicationFactory<API2.Program>();
        var client = factory.CreateClient();
        var returnImages = true;
        var folder = "..\\..\\..\\..\\Unit.Tests\\TestFiles";

        // iterate over all files in folder
        List<string> fileLocations = [];
        foreach (var file in Directory.GetFiles(folder))
        {
            fileLocations.Add(file);
        }

        Directory.CreateDirectory("output");
        var foldersInOutput = Directory.GetDirectories("output").Length;
        foreach (var fileLocation in fileLocations)
        {
            var outputFolder = $"output/{foldersInOutput}/{Path.GetFileNameWithoutExtension(fileLocation)}";
            var request = CreatePostRequest(fileLocation, returnImages);

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var parsedJson = JObject.Parse(responseContent);
            Console.WriteLine(fileLocation);
            var documentId = parsedJson["documentId"]!.ToString();
            string percentages = parsedJson["anonymizedPercentagePerPage"]!.ToString();
            request = CreateGetRequest(documentId);

            response = client.SendAsync(request).Result;
            responseContent = response.Content.ReadAsStringAsync().Result;
            parsedJson = JObject.Parse(responseContent);
            // save images to output folder
            var pages = parsedJson["pages"]!;
            for (int i = 1; i <= pages.Count(); i++)
            {
                var page = pages[i.ToString()]!;
                var imageOriginal = page["Original"]!.ToString();
                var imageResult = page["Result"]!.ToString();
                var imageBytes = Convert.FromBase64String(imageOriginal);
                var imageFilePath = $"{outputFolder}/{i}_original.jpg";
                // ensure folder exists
                Directory.CreateDirectory(Path.GetDirectoryName(imageFilePath)!);
                // create text file with percentages
                if (!File.Exists($"{outputFolder}/percentages.txt"))
                {
                    File.WriteAllText($"{outputFolder}/percentages.txt", percentages);
                }
                File.WriteAllBytes(imageFilePath, imageBytes);
                imageBytes = Convert.FromBase64String(imageResult);
                imageFilePath = $"{outputFolder}/{i}_result.jpg";
                File.WriteAllBytes(imageFilePath, imageBytes);
            }
        }
    }

    internal static HttpRequestMessage CreatePostRequest(string fileLocation, bool returnImages)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");
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
        return request;
    }

    internal static HttpRequestMessage CreateGetRequest(string documentId)
    =>
        new HttpRequestMessage(HttpMethod.Get, "/results")
        {
            Content =
            new StringContent(
                JsonConvert.SerializeObject(
                    new GetDocumentPagesRequest(
                        DocumentId: documentId
                        )
                    ),
                    Encoding.UTF8,
                    "application/json"
                )
        };
    }
