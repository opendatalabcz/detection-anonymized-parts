namespace DAPP.BusinessLogic.Facades
{
	using DAPP.BusinessLogic.Interfaces.Facades;
	using DAPP.BusinessLogic.Interfaces.Operations;

	public sealed class AnalyzerFacade : IAnalyzerFacade
	{
		private readonly ILoadContractsOperation loadContractsOperation;
		private readonly ILoadSingleContractOperation loadSingleContractOperation;
		private readonly IAnalyzeContractsOperation analyzeContractsOperation;
		private readonly IAnalyzeSingleContractOperation analyzeSingleContractOperation;
		public AnalyzerFacade(
			ILoadContractsOperation loadContractsOperation,
			IAnalyzeContractsOperation analyzeContractsOperation,
			ILoadSingleContractOperation loadSingleContractOperation,
			IAnalyzeSingleContractOperation analyzeSingleContractOperation)
		{
			this.loadContractsOperation = loadContractsOperation;
			this.analyzeContractsOperation = analyzeContractsOperation;
			this.loadSingleContractOperation = loadSingleContractOperation;
			this.analyzeSingleContractOperation = analyzeSingleContractOperation;
		}

		public int LoadContract(string contractPath)
		{
			return loadSingleContractOperation.Execute(contractPath);
		}

		public void LoadContracts(string contractsFolderPath)
		{
			loadContractsOperation.Execute(contractsFolderPath);
		}

		public void Run(int contractId)
		{
			if (contractId == -1)
			{
				analyzeContractsOperation.Execute();
			}
			else
			{
				_ = analyzeSingleContractOperation.Execute(contractId);
			}
		}
	}
}
