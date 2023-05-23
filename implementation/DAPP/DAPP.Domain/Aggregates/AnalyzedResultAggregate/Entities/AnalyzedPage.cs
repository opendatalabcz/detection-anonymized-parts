namespace DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities
{
    using DAPP.Domain.Aggregates.ContractAggregate.Entities;
    using System.Text.Json;

    public sealed class AnalyzedPage
    {
        public ContractPage ContractPage { get; set; } = default!;
        public float AnonymizationPercentage { get; set; }


        public dynamic AsJson()
        {
            return JsonSerializer.Serialize(
                new
                {
                    AnonymizationPercentage,
                    ContractPageId = ContractPage.Id,
                }
            );
        }
    }
}
