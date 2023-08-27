using Domain.PageAggregate;
using Domain.PageAggregate.ValueObjects;

namespace Application.Common.Interfaces.Persistance
{
    /// <summary>
    /// Interface for the page repository
    /// </summary>
    public interface IPageRepository
    {
        /// <summary>
        /// Adds a page to the database
        /// </summary>
        /// <param name="p"> The page to add</param>
        /// <returns> The id of the page</returns>
        PageId Add(Page p);
        /// <summary>
        /// Saves an image to the database
        /// </summary>
        /// <param name="value"> The image to save</param>
        /// <returns></returns>
        string SaveImage(byte[] value);
    }

    /// <summary>
    /// The type of image
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// The original image
        /// </summary>
        OriginalImage = 10,

        /// <summary>
        /// The result image
        /// </summary>
        ResultImage = 20,
    }
}
