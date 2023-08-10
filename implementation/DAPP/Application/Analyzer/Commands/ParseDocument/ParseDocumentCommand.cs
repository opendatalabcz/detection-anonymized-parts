using DAPPAnalyzer.Models;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.ParseDocument
{
    public record ParseDocumentCommand(
        DocumentId DocumentId,
        byte[] Data
        ) : IRequest<ErrorOr<(DappPDF, DocumentId)>>;
}
