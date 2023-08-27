using DAPPAnalyzer.Models;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.ParseDocument
{
    /// <summary>
    /// Command to parse a document.
    /// </summary>
    /// <param name="DocumentId"> The document id.</param>
    /// <param name="Data"> The document data.</param>
    public record ParseDocumentCommand(
        DocumentId DocumentId,
        byte[] Data
        ) : IRequest<ErrorOr<(DappPDF, DocumentId)>>;
}
