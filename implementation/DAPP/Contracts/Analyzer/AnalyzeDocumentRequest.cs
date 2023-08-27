namespace Contracts.Analyzer
{
    /// <summary>
    /// Request to analyze a document.
    /// </summary>
    /// <param name="FileLocation"> The file location.</param>
    /// <param name="ReturnImages"> Whether to return images.</param>
    public record AnalyzeDocumentRequest(
        string FileLocation,
        bool ReturnImages = false);
}