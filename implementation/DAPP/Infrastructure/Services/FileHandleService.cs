using Application.Common.Interfaces.Services;
using ErrorOr;

namespace Infrastructure.Services;
/// <summary>
/// A service for handling files
/// </summary>
public class FileHandleService : IFileHandleService
{

    public async Task<ErrorOr<byte[]>> GetBytes(string path)
    {
        byte[] fileBytes;

        try
        {
            // Check if the path is a url
            if (path.StartsWith("http"))
            {
                // Download the file
                using var client = new HttpClient();
                fileBytes = await client.GetByteArrayAsync(path);
            }
            else
            {
                // Read the file
                fileBytes = await File.ReadAllBytesAsync(path);
            }
        }
        catch
        {
            return Domain.Common.Errors.FileHandle.LoadingPdfError;
        }
        return fileBytes;
    }
}
