using Application.Common.Interfaces.Persistance;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    /// <summary>
    /// Repository for the document
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DappDbContext dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"> The database context</param>
        public DocumentRepository(DappDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        ///<inheritdoc/>
        public DocumentId Add(Document document)
        {
            dbContext.Add(document);
            dbContext.SaveChanges();
            return document.Id;
        }

        ///<inheritdoc/>
        public Document? Get(DocumentId id)
        {
            return dbContext.Documents
                .Include(x => x.Pages).SingleOrDefault(x => x.Id == id);
        }

        ///<inheritdoc/>
        public Document? Get(string documentName)
        {
            return dbContext.Documents
                .Include(x => x.Pages).SingleOrDefault(x => x.Name == documentName);
        }

        ///<inheritdoc/>
        public Document? GetByHash(string hash)
        {
            return dbContext.Documents
                .Include(x => x.Pages).SingleOrDefault(x => x.Hash == hash);
        }
    }
}
