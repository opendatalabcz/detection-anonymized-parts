namespace DAPP.Application.Operations
{
	using DAPP.Application.Interfaces;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;

	using ErrorOr;

	public sealed class AddAnalyzedContractOperation
	{
		private readonly IAnalyzedContractRepository repository;

		public AddAnalyzedContractOperation(IAnalyzedContractRepository repository)
		{
			this.repository = repository;
		}

		public ErrorOr<int> Execute(AnalyzedContract result)
		{
			return repository.AddAnalyzedContractResult(result);
		}
	}
}
