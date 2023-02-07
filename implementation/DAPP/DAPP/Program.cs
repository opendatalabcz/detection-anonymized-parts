#define _DEBUG
namespace DAPP
{
	using DAPP.BusinessLogic.Facades;
	using DAPP.BusinessLogic.Operations;
	using DAPP.Controllers;
	using DAPP.Repositories;

	public sealed class Program
	{
		private static void Main(string[] args)
		{
			// load config file
			//var analyzer = new AnalyzerController(ArgsParser.Parse(args));

			// using BusinessLogic and Repository reference because of "Dependency Injection". Can't really go makearound.
			var contractRepository = new ContractRepository();
			var loadSingleConctractOperation = new LoadSingleContractOperation(contractRepository);
			var analyzeSingleContractOperation = new AnalyzeSingleContractOperation(
				contractRepository,
				new()
				{
					new GetBlackBoundingBoxesOperation(),
					new GetBlackBoundingBoxesHighPassFilterOperation(),
					new GetBlackBoundingBoxesSegmentatedFilterOperation(),
				});

			var analyzerFacade = new AnalyzerFacade(
				 new LoadContractsOperation(loadSingleConctractOperation),
				 new AnalyzeContractsOperation(contractRepository, analyzeSingleContractOperation),
				 loadSingleConctractOperation,
				 analyzeSingleContractOperation);
#if _DEBUG
#else
			var contractsFolderPath = @""; // something with args 
#endif
			var analyzer = new AnalyzerController(analyzerFacade);

			if (Config.LoadAllThenAnalyzeAll)
			{
				// loading 
				Console.WriteLine("Loading contracts...");
				Console.WriteLine(Config.ConsoleDelimeter);
				analyzer.LoadContracts(Config.ContractsFolderPath);

				// analyze
				Console.WriteLine("Analyzing contracts...");
				Console.WriteLine(Config.ConsoleDelimeter);
				List<List<Models.AnalyzedContractModel>> results = analyzer.Run();
				Console.WriteLine("Done.");
				Console.WriteLine(Config.ConsoleDelimeter);
				Console.WriteLine("Results:");
				string res = "";
				foreach (List<Models.AnalyzedContractModel> re in results)
				{
					foreach (var result in re)
					{

						res += $"Contract: {result.Name}\n";
						res += $"Number of pages: {result.PagesCount}\n";
						res += $"Method used to determine anonymized parts: {result.FcName}\n";
						res += $"Anonymized area %:\n{result.PercentagesAsString}\n";
						res += Config.ConsoleDelimeter + "\n";
					}
				}
				if (!File.Exists(Config.ResultsFolderPath + "results.txt"))
				{
					using (File.CreateText(Config.ResultsFolderPath + "results.txt"))
					{ }
				}
				File.WriteAllText(Config.ResultsFolderPath + "results.txt", res);
			}
			else // load one and analyze one, then another one and so on
			{
				foreach (string item in Directory.GetFiles(Config.ContractsFolderPath))
				{

					// loading 
					Console.WriteLine($"Loading contract {item.Split('/')[^1]}...");
					Console.WriteLine(Config.ConsoleDelimeter);
					int contractId = analyzer.LoadContract(item);

					Console.WriteLine($"Analyzing contract {item.Split('/')[^1]}...");
					analyzer.Run(contractId);
					Console.WriteLine("Done.");
					Console.WriteLine(Config.ConsoleDelimeter);
					Console.WriteLine("Results:");
					Console.WriteLine(Config.ConsoleDelimeter);
				}
			}
saveOption:
			Console.WriteLine("Would you like to save results ? (y/n)");
			string? r = Console.ReadLine();
			if (r is "y" or "n")
			{
				if (r == "y")
				{
					Console.WriteLine("NOT IMPLEMENTED YET");
				}
			}
			else
			{
				Console.WriteLine("Please type \"y\" for yes or \"n\" for no.");
				goto saveOption;
			}
			Console.WriteLine("|--------------------------------------------------------------|");
		}
	}
}

