namespace DAPPAnalyzer.Interfaces;
public interface IPDFAnalyzer
{
    Task<object> AnalyzeAsync(object data);
}
