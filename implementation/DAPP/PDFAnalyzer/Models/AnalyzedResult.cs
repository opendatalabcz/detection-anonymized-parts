
namespace DAPPAnalyzer.Models
{
    public record AnalyzedResult(
        bool ContainsAnonymizedData,
        float anonymizedPercentage,
        int PageCount,
        Dictionary<int, float> anonymizedPercentagePerPage,
        Dictionary<int, byte[]> resultImages);
}
