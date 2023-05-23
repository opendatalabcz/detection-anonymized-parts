namespace DAPP.API
{
	using DAPP.Application.Facades;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;

	using ErrorOr;

	public sealed class DAPPController
	{
		private readonly ContractFacade contractFacade;

		public DAPPController(ContractFacade contractFacade)
		{
			this.contractFacade = contractFacade;
		}

		public ErrorOr<AnalyzedContract> AnalyzeContract(string path, bool saveImages = false)
		{
			return contractFacade.AnalyzeContract(path, saveImages);
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
