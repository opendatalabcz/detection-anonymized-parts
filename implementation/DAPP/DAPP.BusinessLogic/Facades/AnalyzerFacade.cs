namespace DAPP.BusinessLogic.Facades
{
	using DAPP.BusinessLogic.Interfaces.Facades;
	using DAPP.BusinessLogic.Interfaces.Operations;

	public sealed class AnalyzerFacade : IAnalyzerFacade
	{
		private readonly ILoadContractsOperation loadContractsOperation;
		private readonly IAnalyzeContractsOperation analyzeContractsOperation;
		public AnalyzerFacade(ILoadContractsOperation loadContractsOperation, IAnalyzeContractsOperation analyzeContractsOperation)
		{
			this.loadContractsOperation = loadContractsOperation;
			this.analyzeContractsOperation = analyzeContractsOperation;
		}

		public void LoadContracts(string contractsFolderPath)
		{
			loadContractsOperation.Execute(contractsFolderPath);
		}

		public void Run()
		{
			// case switch decide based on parameter what operation to call
			analyzeContractsOperation.Execute();
		}
	}
}
