using DAPPAnalyzer.Models;

namespace DAPPAnalyzer.Interfaces;
public interface IPDFAnalyzer
{
    /// <summary>
    /// Analyze a pdf file
    /// </summary>
    /// <param name="data"> The pdf file to analyze</param>
    /// <param name="returnImages"> Whether to return the images in the pdf</param>
    /// <returns> The result of the analysis</returns>
    Task<AnalyzedResult> AnalyzeAsync(DappPDF data, bool returnImages = false);
}
