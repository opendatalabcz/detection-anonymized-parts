using Domain.DocumentAggregate.ValueObjects;

namespace Contracts.Results
{
    /// <summary>
    /// Request to get the pages of a document.
    /// </summary>
    /// <param name="DocumentId"> The document id.</param>
    public record GetDocumentPagesRequest(
        string DocumentId);
}