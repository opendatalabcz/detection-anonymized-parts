using Domain.DocumentAggregate.ValueObjects;

namespace Domain.Common.Entities.Analyzer
{
    /// <summary>
    /// The analyzed document data.
    /// </summary>
    /// <param name="DocumentId"> The document id.</param>
    /// <param name="Url"> The url of the document.</param>
    /// <param name="ContainsAnonymizedData"> Whether the document contains anonymized data.</param>
    /// <param name="AnonymizedPercentage"> The percentage of anonymized data.</param>
    /// <param name="PageCount"> The page count of the document.</param>
    /// <param name="AnonymizedPercentagePerPage"> The percentage of anonymized data per page.</param>
    /// <param name="OriginalImages"> The original images.</param>
    /// <param name="ResultImages"> The result images.</param>
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
