using Domain.PageAggregate;
using Domain.PageAggregate.ValueObjects;

namespace Application.Common.Interfaces.Persistance
{
    public interface IPageRepository
    {
        PageId Add(Page p);
        string SaveImage(byte[] value);
    }

    public enum ImageType
    {
        OriginalImage = 10,
        ResultImage = 20,
    }
}
