namespace DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities
{
	using DAPP.Domain.Aggregates.ContractAggregate.Entities;
	using DAPP.Domain.Common;
    using System.Text;

    public sealed class AnalyzedPage
	{
		public ContractPage ContractPage { get; set; }
		public float AnonymizationPercentage { get; set; }

		public string Statistics { get
			{
				var sb = new StringBuilder();
				sb.AppendLine($"Page number {ContractPage.Id}");
				sb.AppendLine($"Anonymization percentage: {AnonymizationPercentage}");
				return sb.ToString();
			}
		}
	}
}
