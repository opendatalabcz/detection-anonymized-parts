
namespace DAPPAnalyzer.Models
{
    public record AnalyzedResult(
        string ContractName,
        bool ContainsAnonymizedData,
        float AnonymizedPercentage,
        int PageCount,
        Dictionary<int, float> AnonymizedPercentagePerPage,
        Dictionary<int, byte[]> OriginalImages,
        Dictionary<int, byte[]> ResultImages);
}
