using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;

namespace Application.Common.Interfaces.Persistance
{
    public interface IDocumentRepository
    {
        /// <summary>
        /// Adds a document to the database
        /// </summary>
        /// <param name="document"> The document to add</param>
        /// <returns> The id of the document</returns>
        DocumentId Add(Document document);

        /// <summary>
        /// Gets a document by id
        /// </summary>
        /// <param name="id"> The id of the document</param>
        /// <returns> The document</returns>
        Document? Get(DocumentId id);
    }
}
