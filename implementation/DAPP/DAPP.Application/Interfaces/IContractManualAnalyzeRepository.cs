namespace DAPP.Application.Interfaces
{
	using DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate;

	public interface IContractManualAnalyzeRepository
	{
		int AddManuallyAnalyzedContract(ManuallyAnalyzedResult result);
	}
}
