using Domain.DocumentAggregate.ValueObjects;

namespace Domain.Common.Entities.Analyzer
{
    public record AnalyzedDocumentData(
           DocumentId DocumentId,
           string Url,
           bool ContainsAnonymizedData,
           float AnonymizedPercentage,
           int PageCount,
           Dictionary<int, float> AnonymizedPercentagePerPage,
           Dictionary<int, byte[]> OriginalImages,
           Dictionary<int, byte[]> ResultImages);
}
