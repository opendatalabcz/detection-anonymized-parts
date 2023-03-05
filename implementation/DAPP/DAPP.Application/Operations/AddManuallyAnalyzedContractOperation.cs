namespace DAPP.Application.Operations
{
	using DAPP.Application.Interfaces;
	using DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate;

	public sealed class AddManuallyAnalyzedContractOperation
	{
		private readonly IContractManualAnalyzeRepository repo;
		public int Execute(ManuallyAnalyzedResult r)
		{
			return repo.AddManuallyAnalyzedContract(r);
		}
	}
}
