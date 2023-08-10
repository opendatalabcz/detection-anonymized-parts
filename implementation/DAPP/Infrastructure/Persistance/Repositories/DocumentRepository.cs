using Application.Common.Interfaces.Persistance;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DappDbContext dbContext;

        public DocumentRepository(DappDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DocumentId Add(Document document)
        {
            dbContext.Add(document);
            dbContext.SaveChanges();
            return document.Id;
        }

        public Document? Get(DocumentId id)
        {
            return dbContext.Documents
                .Include(x => x.Pages).SingleOrDefault(x => x.Id == id);
        }

        public Document? Get(string documentName)
        {
            return dbContext.Documents
                .Include(x => x.Pages).SingleOrDefault(x => x.Name == documentName);
        }

        public Document? GetByHash(string hash)
        {
            return dbContext.Documents
                .Include(x => x.Pages).SingleOrDefault(x => x.Hash == hash);
        }
    }
}
