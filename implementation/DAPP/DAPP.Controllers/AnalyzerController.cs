namespace DAPP.Controllers
{
	using DAPP.BusinessLogic.Interfaces.Facades;
	using DAPP.Models;

	public sealed class AnalyzerController
	{
		private readonly IAnalyzerFacade analyzerFacade;

		public AnalyzerController(IAnalyzerFacade analyzerFacade)
		{
			this.analyzerFacade = analyzerFacade;
		}

		public void LoadContracts(string contractsFolderPath)
		{
			analyzerFacade.LoadContracts(contractsFolderPath);
		}

		public int LoadContract(string contractPath)
		{
			return analyzerFacade.LoadContract(contractPath);
		}

		public List<List<AnalyzedContractModel>> Run(int contractId = -1)
		{
			return analyzerFacade.Run(contractId);
		}
	}
}