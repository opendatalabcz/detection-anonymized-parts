using API.Errors;
using ErrorOr;
namespace API.Services
{
    public static class FileHandleService
    {
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
            catch (Exception e)
            {
                return ApiErrors.LoadingPdfError;
            }
            return fileBytes;
        }
    }
}
