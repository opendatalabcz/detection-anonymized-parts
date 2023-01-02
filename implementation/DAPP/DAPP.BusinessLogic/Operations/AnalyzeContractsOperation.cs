namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;

	public sealed class AnalyzeContractsOperation : IAnalyzeContractsOperation
	{
		private readonly IContractRepository contractRepository;
		private readonly IAnalyzeSingleContractOperation analyzeSingleContractOperation;

		public AnalyzeContractsOperation(IContractRepository contractRepository,
			IAnalyzeSingleContractOperation analyzeSingleContractOperation)
		{
			this.contractRepository = contractRepository;
			this.analyzeSingleContractOperation = analyzeSingleContractOperation;
		}

		public void Execute()
		{
			foreach (Entities.Contract contract in contractRepository.GetAllContracts())
			{
				analyzeSingleContractOperation.Execute(contract);
			}
		}
	}
}
