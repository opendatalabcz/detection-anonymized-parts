namespace Contracts.Analyzer
{
    public record AnalyzeDocumentResponse
        (
            Guid DocumentId,
            string Url,
            bool ContainsAnonymizedData,
            float AnonymizedPercentage,
            int PageCount,
            Dictionary<int, float> AnonymizedPercentagePerPage,
            Dictionary<int, byte[]>? OriginalImages,
            Dictionary<int, byte[]>? ResultImages
        );
}
