namespace DAPP.Application.Operations
{
	using DAPP.Application.Interfaces;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;

	public sealed class GetAnalyzedContractsOperation
	{
		private readonly IAnalyzedContractRepository repository;

		public GetAnalyzedContractsOperation(IAnalyzedContractRepository repository)
		{
			this.repository = repository;
		}

		public IEnumerable<AnalyzedContract> Execute()
		{
			return repository.GetAnalyzedContracts();
		}
	}
}
