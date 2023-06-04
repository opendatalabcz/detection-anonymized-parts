namespace DAPPAnalyzer.Models;

/// <summary>
/// Represents the result of an analysis of a PDF file 
/// as response to a <see cref="FileLocationRequest"/>
/// </summary>
/// <param name="ContractName"></param>
/// <param name="url"></param>
/// <param name="ContainsAnonymizedData"></param>
/// <param name="AnonymizedPercentage"></param>
/// <param name="PageCount"></param>
/// <param name="AnonymizedPercentagePerPage"></param>
/// <param name="OriginalImages"></param>
/// <param name="ResultImages"></param>
public record AnalyzedResult(
    string ContractName,
    string url,
    bool ContainsAnonymizedData,
    float AnonymizedPercentage,
    int PageCount,
    Dictionary<int, float> AnonymizedPercentagePerPage,
    Dictionary<int, byte[]> OriginalImages,
    Dictionary<int, byte[]> ResultImages);
