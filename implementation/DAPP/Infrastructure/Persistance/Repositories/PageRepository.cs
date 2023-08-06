using Application.Common.Interfaces.Persistance;
using Domain.PageAggregate;
using Domain.PageAggregate.ValueObjects;

namespace Infrastructure.Persistance.Repositories
{
    public class PageRepository : IPageRepository
    {
        public Task<PageId> Add(Page p)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveImage(byte[] value, ImageType type)
        {
            throw new NotImplementedException();
        }
    }
}
