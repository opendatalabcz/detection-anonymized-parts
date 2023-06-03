using DAPPAnalyzer.Models;

namespace DAPPAnalyzer.Interfaces;
public interface IPDFAnalyzer
{
    Task<object> AnalyzeAsync(DappPDF data);
}
