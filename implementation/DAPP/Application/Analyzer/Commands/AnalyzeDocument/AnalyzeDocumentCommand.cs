using DAPPAnalyzer.Models;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.AnalyzeDocument
{
    public record AnalyzeDocumentCommand(
        DappPDF DappPdf,
        DocumentId DocumentId,
        bool ReturnImages = false) : IRequest<ErrorOr<Document>>;
}
