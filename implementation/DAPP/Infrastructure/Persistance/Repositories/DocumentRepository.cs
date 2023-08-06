using Application.Common.Interfaces.Persistance;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;

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
            return dbContext.Documents.SingleOrDefault(x => x.Id == id);
        }
    }
}
