using Domain.Common.Entities.Analyzer;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Queries.GetAnalyzedDocument
{
    public record GetAnalyzedDocumentDataQuery(DocumentId Id) : IRequest<ErrorOr<AnalyzedDocumentData>>;
}
