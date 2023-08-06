using Domain.PageAggregate;
using Domain.PageAggregate.ValueObjects;

namespace Application.Common.Interfaces.Persistance
{
    public interface IPageRepository
    {
        Task<PageId> Add(Page p);
        Task<string> SaveImage(byte[] value, ImageType type);
    }

    public enum ImageType
    {
        OriginalImage = 10,
        ResultImage = 20,
    }
}
