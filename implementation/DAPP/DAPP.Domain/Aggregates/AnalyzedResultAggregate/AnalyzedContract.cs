namespace DAPP.Domain.Aggregates.AnalyzedResultAggregate
{
    using DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities;
    using DAPP.Domain.Aggregates.ContractAggregate;

    using ErrorOr;
    using System.Text.Json;

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

        public dynamic ToJson()
        {
            var d = new Dictionary<int, object>();
            var totalStats = 0.0f;
            int i = 1;
            foreach (var page in AnalyzedPages)
            {
                page.Switch(
                    page =>
                {
                    d.Add(i++, page.AnonymizationPercentage);
                    totalStats += page.AnonymizationPercentage;
                },
                    error =>
                {
                    d.Add(i++, page.FirstError);
                });
            }
            return JsonSerializer.Serialize(new { ContractName = Contract.Name, PagesCount = AnalyzedPages.Count, IndividualPages = d, AveragePerPage = totalStats });
        }
    }
}
