using Domain.DocumentAggregate;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Commands.AnalyzeDocument
{
    public record AnalyzeDocumentCommand(
        string FileLocation,
        bool ReturnImages = false) : IRequest<ErrorOr<Document>>;
}
