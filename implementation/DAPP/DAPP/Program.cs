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
			//var analyzer = new AnalyzerController(ArgsParser.Parse(args));

			// using BusinessLogic and Repository reference because of "Dependency Injection". Can't really go makearound.
			var contractRepository = new ContractRepository();
			(int, int) pxPerCmDensity = (100, 100);
			var analyzerFacade = new AnalyzerFacade(
				 new LoadContractsOperation(contractRepository, pxPerCmDensity),
				 new AnalyzeContractsOperation(contractRepository,
				 new AnalyzeSingleContractOperation(contractRepository)));
#if _DEBUG
			string contractsFolderPath = @"../../../../TestData/pdfs/";
#else
			var contractsFolderPath = @""; // something with args 
#endif
			var analyzer = new AnalyzerController(analyzerFacade);
			Console.WriteLine("Loading contracts...");
			Console.WriteLine("|--------------------------------------------------------------|");
			analyzer.LoadContracts(contractsFolderPath);
			Console.WriteLine("Analyzing contracts...");
			Console.WriteLine("|--------------------------------------------------------------|");
			analyzer.Run();
			Console.WriteLine("Done.");
			Console.WriteLine("|--------------------------------------------------------------|");
			Console.WriteLine("Results:");
			Console.WriteLine("|--------------------------------------------------------------|");
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