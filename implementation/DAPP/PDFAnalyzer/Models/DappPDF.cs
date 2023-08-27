namespace DAPPAnalyzer.Models;

/// <summary>
/// Class representing a PDF file
/// </summary>
public class DappPDF
{
    /// <summary>
    /// Gets or sets the name of the contract
    /// </summary>
    public string ContractName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the url of the PDF file
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// Gets or sets the pages of the PDF file
    /// </summary>
    public List<Mat> Pages { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the PDF file has been analyzed
    /// </summary>
    public bool Analyzed { get; set; } = false;

    /// <summary>
    /// Creates a DappPDF object from a byte array
    /// </summary>
    /// <param name="data"> The byte array of the PDF file</param>
    /// <param name="contractName"> The name of the contract</param>
    /// <param name="url"> The url of the PDF file</param>
    /// <returns> A DappPDF object</returns>
    public static async Task<DappPDF> Create(byte[] data, string contractName, string url)
    {
        var pdf = new DappPDF
        {
            Url = url,
            ContractName = contractName,
            Pages = await Task.Run(() => ConvertToImages(data))
        };
        return pdf;
    }

    /// <summary>
    /// Converts a PDF file to a list of images
    /// </summary>
    /// <param name="pdfBytes"> The byte array of the PDF file</param>
    /// <returns> A list of images</returns>
    private static List<Mat> ConvertToImages(byte[] pdfBytes)
    {
        var result = new List<Mat>();
        var tempFilePath = Path.GetTempFileName();

        try
        {
            // Save the PDF data to a temporary file
            File.WriteAllBytes(tempFilePath, pdfBytes);

            using var images = new MagickImageCollection();
            // Read the temporary PDF file
            images.Read(tempFilePath);

            foreach (var image in images)
            {
                // Convert the MagickImage image to a byte array
                byte[] bytes = image.ToByteArray(MagickFormat.Bmp);

                // Create a Mat object using the byte array
                Mat mat = Cv2.ImDecode(bytes, ImreadModes.Unchanged);
                result.Add(mat);
            }
        }
        finally
        {
            // Clean up the temporary file
            File.Delete(tempFilePath);
        }

        return result;
    }
}