namespace DAPP.API
{
	using DAPP.Application.Facades;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;
	using DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate;

	using ErrorOr;

	public sealed class DAPPController
	{
		private readonly ContractFacade contractFacade;

		public DAPPController(ContractFacade contractFacade)
		{
			this.contractFacade = contractFacade;
		}

		public ErrorOr<AnalyzedContract> AnalyzeContract(string path)
		{
			return contractFacade.AnalyzeContract(path);
		}

		public ErrorOr<int> AddManuallyAnalyzedContract(ManuallyAnalyzedResult r)
		{
			return contractFacade.AddManuallyAnalyzedContract(r);
		}

		public ErrorOr<int> AddAnalyzedContract(AnalyzedContract r)
		{
			return contractFacade.AddAnalyzedContract(r);
		}

		public IEnumerable<AnalyzedContract> GetAnalyzedContracts()
		{
			return contractFacade.GetAnalyzedContracts();
		}
	}
}
