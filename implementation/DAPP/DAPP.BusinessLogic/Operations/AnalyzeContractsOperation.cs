namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Models;

	using System.Collections.Generic;

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
		public List<List<AnalyzedContractModel>> Execute()
		{
			var result = new List<List<AnalyzedContractModel>>();
			foreach (Entities.Contract contract in contractRepository.GetAllContracts())
			{
				result.Add(analyzeSingleContractOperation.Execute(contract.Id));
			}
			return result;
		}
	}
}
