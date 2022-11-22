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
			var analyzerFacade = new AnalyzerFacade(
				 new LoadContractsOperation(contractRepository),
				 new AnalyzeContractsOperation(contractRepository,
				 new AnalyzeSingleContractOperation(contractRepository)));
#if _DEBUG
			string contractsFolderPath = @"../../../../TestData/";
#else
			var contractsFolderPath = @""; // something with args 
#endif
			var analyzer = new AnalyzerController(analyzerFacade);
			Console.WriteLine("Loading contracts...");
			analyzer.LoadContracts(contractsFolderPath);
			Console.WriteLine("Analyzing contracts...");
			analyzer.Run();
			Console.WriteLine("Done.");

		}
	}
}