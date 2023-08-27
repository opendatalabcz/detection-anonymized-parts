using Domain.Common.Entities.Analyzer;
using Domain.DocumentAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Analyzer.Queries.GetAnalyzedDocument
{
    /// <summary>
    /// Query to get the analyzed document data.
    /// </summary>
    /// <param name="Id"></param>
    public record GetAnalyzedDocumentDataQuery(DocumentId Id) : IRequest<ErrorOr<AnalyzedDocumentData>>;
}
