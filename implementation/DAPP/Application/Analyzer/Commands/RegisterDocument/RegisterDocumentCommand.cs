using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.RegisterDocument
{
    public record RegisterDocumentCommand
    (string FileLocation) : IRequest<ErrorOr<(DocumentId, byte[])>>;
}
