
namespace DAPP.Application.Facades
{
	using System.Collections.Generic;

	using DAPP.Application.Operations;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;
	using DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate;

	using ErrorOr;

	public sealed class ContractFacade
	{
		private readonly AnalyzeContractOperation analyzeContractOperation;
		private readonly CreateContractOperation createContractOperation;
		private readonly AddManuallyAnalyzedContractOperation addManuallyAnalyzedContractOperation;
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

		public int AddManuallyAnalyzedContract(ManuallyAnalyzedResult r)
		{
			return addManuallyAnalyzedContractOperation.Execute(r);
		}

		public ErrorOr<AnalyzedContract> AnalyzeContract(string filepath)
		{
			var contract = createContractOperation.Execute(filepath);

			return
				contract.IsError
				?
				contract.Errors
				:
				analyzeContractOperation.Execute(contract.Value);
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
