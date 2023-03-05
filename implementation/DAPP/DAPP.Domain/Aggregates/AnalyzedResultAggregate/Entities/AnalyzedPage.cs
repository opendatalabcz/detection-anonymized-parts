namespace DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities
{
	using DAPP.Domain.Aggregates.ContractAggregate.Entities;
	using DAPP.Domain.Common;

	public sealed class AnalyzedPage
	{
		public ContractPage ContractPage { get; set; }

		public AnonymizationTypeEnum AnonymizationType { get; set; }
		public bool IsGrayscale { get; set; }
	}
}
