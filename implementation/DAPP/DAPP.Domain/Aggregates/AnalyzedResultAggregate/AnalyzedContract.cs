namespace DAPP.Domain.Aggregates.AnalyzedResultAggregate
{
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities;
	using DAPP.Domain.Aggregates.ContractAggregate;

	using ErrorOr;

	public sealed class AnalyzedContract
	{
		public required Contract Contract { get; set; }
		public int Id { get; set; }

		public required List<ErrorOr<AnalyzedPage>> AnalyzedPages { get; set; }

		public void Deconstruct(out Contract c, out List<ErrorOr<AnalyzedPage>> pages)
		{
			c = Contract;
			pages = AnalyzedPages;
		}
	}
}
