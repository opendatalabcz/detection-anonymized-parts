using ErrorOr;

namespace Application.Common.Interfaces.Services
{
    public interface IFileHandleService
    {
        /// <summary>
        /// Gets the bytes of a file
        /// </summary>
        /// <param name="path"> The path to the file, can be a url or a path to a file on disk</param>
        /// <returns> The bytes of the file or an error</returns>
        Task<ErrorOr<byte[]>> GetBytes(string path);
    }
}
