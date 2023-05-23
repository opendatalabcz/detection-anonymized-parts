
namespace DAPP.Application.Facades
{
	using System.Collections.Generic;

	using DAPP.Application.Operations;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;

	using ErrorOr;

	public sealed class ContractFacade
	{
		private readonly AnalyzeContractOperation analyzeContractOperation;
		private readonly CreateContractOperation createContractOperation;
		private readonly AddAnalyzedContractOperation addAnalyzedContractOperation;
		private readonly GetAnalyzedContractsOperation getAnalyzedContractsOperation;
		public ContractFacade(
			AnalyzeContractOperation analyzeContractOperation,
			CreateContractOperation createContractOperation,
			AddAnalyzedContractOperation addAnalyzedContractOperation,
			GetAnalyzedContractsOperation getAnalyzedContractsOperation)
		{
			this.analyzeContractOperation = analyzeContractOperation;
			this.createContractOperation = createContractOperation;
			this.addAnalyzedContractOperation = addAnalyzedContractOperation;
			this.getAnalyzedContractsOperation = getAnalyzedContractsOperation;
		}

		public ErrorOr<AnalyzedContract> AnalyzeContract(string filepath, bool saveImages)
		{
			var contract = createContractOperation.Execute(filepath);

			return
				contract.IsError
				?
				contract.Errors
				:
				analyzeContractOperation.Execute(contract.Value, saveImages);
		}

		public ErrorOr<int> AddAnalyzedContract(AnalyzedContract contract)
		{
			return addAnalyzedContractOperation.Execute(contract);
		}

		public IEnumerable<AnalyzedContract> GetAnalyzedContracts()
		{
			return getAnalyzedContractsOperation.Execute();
		}
	}
}
