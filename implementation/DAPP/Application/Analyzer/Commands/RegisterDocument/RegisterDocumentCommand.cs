using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.RegisterDocument
{
    /// <summary>
    /// Command to register a document.
    /// </summary>
    /// <param name="FileLocation"> The file location.</param>
    public record RegisterDocumentCommand
    (string FileLocation) : IRequest<ErrorOr<(DocumentId, byte[])>>;
}
