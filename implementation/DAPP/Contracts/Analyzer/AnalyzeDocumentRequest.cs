namespace Contracts.Analyzer
{
    public record AnalyzeDocumentRequest(
        string FileLocation,
        bool ReturnImages = false);
}