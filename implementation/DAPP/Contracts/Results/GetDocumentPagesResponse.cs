using Domain.PageAggregate.ValueObjects;

namespace Contracts.Results
{
    /// <summary>
    /// Response for the GetDocumentPagesCommand.
    /// </summary>
    /// <param name="DocumentId"></param>
    /// <param name="Url"></param>
    /// <param name="Pages"></param>
    public record GetDocumentPagesResponse(
        Guid DocumentId,
        string Url,
        Dictionary<int, Dictionary<string, byte[]>> Pages);
}
