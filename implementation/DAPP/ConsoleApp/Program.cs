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
            // init api
            var factory = new WebApplicationFactory<API2.Program>();
            var client = factory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");


            var parseArgs = new ArgumentParser(args);

            var fileLocation = /* argparse */ "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";
            var returnImages = /* argparse */ true;


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
}
