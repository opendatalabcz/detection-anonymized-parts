using ImageMagick;
using OpenCvSharp;

namespace DAPPAnalyzer.Models
{
    /// <summary>
    /// Class representing a PDF file
    /// </summary>
    public class DappPDF
    {
        /// <summary>
        /// Gets or sets the pages of the PDF file
        /// </summary>
        public List<Mat> Pages { get; set; } = default!;

        /// <summary>
        /// Creates a DappPDF object from a byte array
        /// </summary>
        /// <param name="data"> The byte array of the PDF file</param>
        /// <returns> A DappPDF object</returns>
        public static async Task<DappPDF> Create(byte[] data)
        {
            var pdf = new DappPDF();
            pdf.Pages = await Task.Run(() => pdf.ConvertToImages(data));
            return pdf;
        }

        /// <summary>
        /// Converts a PDF file to a list of images
        /// </summary>
        /// <param name="pdfPath"> The byte array of the PDF file</param>
        /// <returns> A list of images</returns>
        private List<Mat> ConvertToImages(byte[] pdfPath)
        {
            var result = new List<Mat>();
            using (var images = new MagickImageCollection())
            {
                images.Read(pdfPath);
                foreach (var image in images)
                {
                    Mat mat = new Mat();
                    mat = Cv2.ImDecode(image.ToByteArray(), ImreadModes.Color);
                    result.Add(mat);
                }
            }
            return result;
        }
    }
}