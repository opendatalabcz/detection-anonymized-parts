namespace API.Services;

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
            return ApiErrors.LoadingPdfError;
        }
        return fileBytes;
    }
}
