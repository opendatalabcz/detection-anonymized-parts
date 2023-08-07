using Application.Common.Interfaces.Persistance;
using Domain.PageAggregate;
using Domain.PageAggregate.ValueObjects;

namespace Infrastructure.Persistance.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly DappDbContext dbContext;

        public PageRepository(DappDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public PageId Add(Page p)
        {
            dbContext.Add(p);
            dbContext.SaveChanges();
            return p.Id;
        }

        public string SaveImage(byte[] value)
        {
            var path = dbContext.StoragePath;
            var fileName = Guid.NewGuid().ToString();
            var fullPath = Path.Combine(path, fileName + ".jpg");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(fullPath, value);
            return fullPath;
        }
    }
}
