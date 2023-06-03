using DAPPAnalyzer.Interfaces;
using DAPPAnalyzer.Models;
using System.Security.Cryptography;

namespace DAPPAnalyzer.Services;
public class PDFAnalyzer : IPDFAnalyzer
{
    public async Task<object> AnalyzeAsync(DappPDF pdf)
    {
        return pdf.Pages.Count;
    }
}
