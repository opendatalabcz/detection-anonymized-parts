namespace DAPP.Domain.Aggregates.AnalyzedResultAggregate
{
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities;
	using DAPP.Domain.Aggregates.ContractAggregate;

	using ErrorOr;
    using System.Text;

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

		public string Statistics { get {
				var sb = new StringBuilder();
				sb.AppendLine($"Contract: {Contract.Name}");
				sb.AppendLine($"Total pages: {AnalyzedPages.Count}");
				var stats = AnalyzedPages.Where(x => !x.IsError).Select(x => x.Value);
				var totalStats = 0.0f;
				foreach (var stat in stats)
				{
					sb.Append(stat.Statistics);
					totalStats += stat.AnonymizationPercentage;
				}
				sb.AppendLine($"Total average per page: {totalStats / stats.Count()}");
				return sb.ToString();
			}
		}
	}
}
