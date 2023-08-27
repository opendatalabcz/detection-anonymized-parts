using DAPPAnalyzer.Models;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.AnalyzeDocument
{
    /// <summary>
    /// Command to analyze a document.
    /// </summary>
    /// <param name="DappPdf"> The DappPDF.</param>
    /// <param name="DocumentId"> The document id.</param>
    /// <param name="ReturnImages"> Whether to return images.</param>
    public record AnalyzeDocumentCommand(
        DappPDF DappPdf,
        DocumentId DocumentId,
        bool ReturnImages = false) : IRequest<ErrorOr<Document>>;
}
