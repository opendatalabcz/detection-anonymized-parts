using API.Errors;
using ErrorOr;
namespace API.Services
{
    /// <summary>
    /// A service for handling files
    /// </summary>
    public static class FileHandleService
    {
        /// <summary>
        /// Gets the bytes of a file
        /// </summary>
        /// <param name="path"> The path to the file, can be a url or a path to a file on disk</param>
        /// <returns> The bytes of the file or an error</returns>
        public static async Task<ErrorOr<byte[]>> GetBytes(string path)
        {
            byte[] fileBytes;

            try
            {
                // načítanie súboru, uložiť na disk do temp
                if (path.StartsWith("http"))
                {
                    // súbor je na internete
                    using var client = new HttpClient();
                    fileBytes = await client.GetByteArrayAsync(path);
                }
                else
                {
                    // súbor je na disku
                    fileBytes = await File.ReadAllBytesAsync(path);
                }
            }
            catch
            {
                return ApiErrors.LoadingPdfError;
            }
            return fileBytes;
        }
    }
}
