namespace DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate
{
	using DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate.Entities;
	using DAPP.Domain.Common;

	public sealed class ManuallyAnalyzedResult
	{
		public int Id { get; set; }
		public required string ContractName { get; set; }

		public required ScanTypeEnum TypeOfScan { get; set; }

		public required List<ManuallyAnalyzedPage> ManuallyAnalyzedPages { get; set; }
	}
}