using Application.Common.Interfaces.Persistance;
using Domain.PageAggregate;
using Domain.PageAggregate.ValueObjects;

namespace Infrastructure.Persistance.Repositories
{
    /// <summary>
    /// The page repository
    /// </summary>
    public class PageRepository : IPageRepository
    {
        private readonly DappDbContext dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">    The database context</param>
        public PageRepository(DappDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        ///<inheritdoc/>
        public PageId Add(Page p)
        {
            dbContext.Add(p);
            dbContext.SaveChanges();
            return p.Id;
        }

        ///<inheritdoc/>
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
