namespace DAPP.Console
{
    using DAPP.API;
    using DAPP.DI;

    using Microsoft.Extensions.DependencyInjection;
    using System.Text;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = DependencyInjectionService.ConfigureServices(new ServiceCollection());

            using var scope = services.CreateScope();
            var controller = scope.ServiceProvider.GetRequiredService<DAPPController>();

            // Args 
            if (args.Length == 0)
            {
                System.Console.WriteLine("No arguments provided.");
                return;
            }
            string file = args[0];
            bool saveImages = false;
            if ((args.Length > 1 && args[1] == "--saveImages") || args[1] == "-saveImages" || args[1] == "-s")
            {
                saveImages = true;
            }

            StringBuilder results = new();

            if (File.Exists(file))
            {
                var result = controller.AnalyzeContract(file, saveImages);
                controller.AddAnalyzedContract(result.Value);
            }
            else if (Directory.Exists(file))
            {
                // file is actually directory
                foreach (var fname in Directory.GetFiles(file))
                {
                    controller.AddAnalyzedContract(controller.AnalyzeContract(fname, saveImages).Value);
                }
            }
            foreach (var c in controller.GetAnalyzedContracts())
            {
                System.Console.WriteLine(c.ToJson());
                results.AppendLine(c.ToJson());
            }
            File.WriteAllText(@"..\..\..\..\Results\results.txt", results.ToString());

#if TEST
            // analyze test data
            foreach (var result in AnalyzeTestData(controller))
            {
                if (!result.IsError)
                {
                    controller.AddAnalyzedContract(result.Value);
                }
            }

            var totalStats = new StringBuilder();
            foreach (var analyzedContract in controller.GetAnalyzedContracts())
            {
                (Contract c, List<ErrorOr<AnalyzedPage>> pages) = analyzedContract;
                // print to console
                var stats = analyzedContract.ToJson();
                Console.WriteLine(stats);
                // save to results.txt
                totalStats.AppendLine(stats);
            }
            File.WriteAllText(@"..\..\..\..\Results\results.txt", totalStats.ToString());

        }
#endif
#if TEST
        private static IEnumerable<ErrorOr<AnalyzedContract>> AnalyzeTestData(DAPPController controller)
        {
            DirectoryInfo pdfsDirectoryInfo = new DirectoryInfo(@"..\..\..\..\TestData\pdfs\");
            foreach (var file in pdfsDirectoryInfo.GetFiles())
            {
                yield return controller.AnalyzeContract(file.FullName);
            }

            //DirectoryInfo samplesDirectoryInfo = new DirectoryInfo(@"..\..\..\..\TestData\samples\");
            //foreach (var file in samplesDirectoryInfo.GetFiles())
            //{
            //    yield return controller.AnalyzeContract(file.FullName);
            //}

        }
#endif

        }
    }
}