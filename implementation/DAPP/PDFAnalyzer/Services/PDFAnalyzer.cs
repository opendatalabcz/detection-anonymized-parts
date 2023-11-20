using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("Unit.Tests")]
namespace DAPPAnalyzer.Services;
/// <summary>
/// A service for analyzing PDFs.
/// </summary>
public class PDFAnalyzer
{
    /// <summary>
    /// Analyzes a PDF.
    /// </summary>
    /// <param name="pdf"> The PDF to analyze.</param>
    /// <returns> The analyzed result.</returns>
    public static async Task<AnalyzedResult> AnalyzeAsync(DappPDF pdf)
    {
        var anonymizedPercentage = 0f;
        var anonymizedPercentagePerPage = new Dictionary<int, float>();
        var containsAnonymizedData = false;
        var originalImages = new Dictionary<int, byte[]>();
        var anonymizedImages = new Dictionary<int, byte[]>();

        return await Task.Run(() =>
        {
            int i = 0;
            foreach (var page in pdf.Pages)
            {
                (Mat anonymizedParts, bool cad, float ap) = AnalyzePage(page);
                containsAnonymizedData |= cad;
                anonymizedPercentagePerPage[i++] = ap;
                originalImages[i] = page.ToBytes(".jpg");
                var masked = MaskOriginal(page, anonymizedParts);
                anonymizedImages[i] = masked.ToBytes(".jpg");
            }
            return new AnalyzedResult(
               pdf.ContractName,
               pdf.Url,
               containsAnonymizedData,
               anonymizedPercentage,
               pdf.Pages.Count,
               anonymizedPercentagePerPage,
               originalImages,
               anonymizedImages);
        });
    }

    /// <summary>
    /// Analyzes a page.
    /// </summary>
    /// <param name="page"> The page to analyze.</param>
    /// <returns> The analyzed result.</returns>
    internal static (Mat anonymizedParts, bool containsAnonymizedData, float anonymizedPercentage) AnalyzePage(Mat page)
    {
        page = CorrectNonUniformIllumination(page);
        var anonymizedParts = GetAnonymizedParts(page);
        var anonymizedPercentage = (float)((page.Rows * page.Cols) - anonymizedParts.CountNonZero()) / (page.Rows * page.Cols);
        var containsAnonymizedData = anonymizedPercentage > 0.01;
        return (anonymizedParts, containsAnonymizedData, anonymizedPercentage);
    }

    /// <summary>
    /// Masks the original image with the anonymized parts.
    /// </summary>
    /// <param name="img"> The original image.</param>
    /// <param name="erodeValue">The erode value.</param>
    /// <param name="dilateValue"> The dilate value.</param>
    /// <returns> The masked image.</returns>
    internal static Mat GetAnonymizedParts(Mat img, int erodeValue = 8, int dilateValue = 4)
    {

        var coloredPixels = ColoredPixels(img);
        // Increase their saturation 
        var imgSaturatedColors = img;
        if (coloredPixels.Count != 0)
        {
           imgSaturatedColors = IncreaseSaturation(img, coloredPixels, 100);
        }
        // Create structuring element
        Mat se = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        var dilated = Dilate(imgSaturatedColors, se);

        var dilated_threshold = Threshold(dilated, 20);
        var dilated2 = Dilate(dilated_threshold, se);


        var result = dilated2;
        for (int i = 0; i < erodeValue; i++)
        {
            result = Erode(result, se);
        }
        for (int i = 0; i < dilateValue; i++)
        {
            result = Dilate(result, se);
        }

        return result;
    }


    /// <summary>
    /// Implementation of nonuniform illumination correction.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="kernelSize"></param>
    /// <returns> The corrected image.</returns>
    internal static Mat CorrectNonUniformIllumination(Mat image, int kernelSize = 15)
    {
        // Convert to grayscale
        Mat grayscaleImage = new Mat();
        Cv2.CvtColor(image, grayscaleImage, ColorConversionCodes.BGR2GRAY);

        // Estimate the background illumination
        Mat background = new Mat();
        Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(kernelSize, kernelSize));
        Cv2.MorphologyEx(grayscaleImage, background, MorphTypes.Close, kernel);

        // Subtract the background
        Mat subtracted = new Mat();
        Cv2.Subtract(grayscaleImage, background, subtracted);

        // Normalize the image
        Mat normalized = new Mat();
        Cv2.Normalize(subtracted, normalized, 0, 255, NormTypes.MinMax);

        return normalized;
    }

    /// <summary>
    /// Gets the colored pixels.
    /// </summary>
    /// <param name="img"> The image.</param>
    /// <returns>List of colored pixels.</returns>
    internal static List<Point> ColoredPixels(Mat img)
    {
        List<Point> coloredPixels = new();
        // iterate through each pixel and check if it is colorful
        for (int i = 0; i < img.Rows; i++)
        {
            for (int j = 0; j < img.Cols; j++)
            {
                if (
                    Math.Abs(img.At<Vec3b>(i, j)[0] - img.At<Vec3b>(i, j)[1]) > 35
                || Math.Abs(img.At<Vec3b>(i, j)[0] - img.At<Vec3b>(i, j)[2]) > 35
                || Math.Abs(img.At<Vec3b>(i, j)[1] - img.At<Vec3b>(i, j)[2]) > 35)
                {
                    coloredPixels.Add(new Point(i, j));
                }
            }
        }
        if (coloredPixels.Count < 40)
        {
            return new List<Point>();
        }

        return coloredPixels;
    }
    /// <summary>
    /// Dilates the image.
    /// </summary>
    /// <param name="src"> The image.</param>
    /// <param name="pattern"> The pattern.</param>
    /// <returns> The dilated image.</returns>
    internal static Mat Dilate(Mat src, Mat pattern)
    {
        Mat result = new();
        Cv2.Dilate(src, result, pattern);
        return result;
    }
    /// <summary>
    /// Erodes the image.
    /// </summary>
    /// <param name="src"> The image.</param>
    /// <param name="pattern"> The pattern.</param>
    /// <returns> The eroded image.</returns>
    internal static Mat Erode(Mat src, Mat pattern)
    {
        Mat result = new();
        Cv2.Erode(src, result, pattern);
        return result;
    }

    /// <summary>
    /// Thresholds the image.
    /// </summary>
    /// <param name="src"> The image.</param>
    /// <param name="val"> The threshold value.</param>
    /// <returns></returns>
    internal static Mat Threshold(Mat src, int val)
    {
        Mat gray = new();
        if (src.Channels() == 1)
        {
            gray = src.Clone();
        }
        else
        {
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
        }
        Mat result = new();
        Cv2.Threshold(gray, result, val, 255, ThresholdTypes.Otsu);

        return result;
    }

    /// <summary>
    /// Increases the saturation of the image.
    /// </summary>
    /// <param name="src"> The image.</param>
    /// <param name="points"> The points to increase the saturation.</param>
    /// <param name="value"> The value to increase the saturation.</param>
    /// <returns> The image with increased saturation.</returns>
    internal static Mat IncreaseSaturation(Mat src, List<Point> points, double value)
    {
        if (points.Count == 0)
        {
            return src;
        }

        Mat hsv = new();
        Cv2.CvtColor(src, hsv, ColorConversionCodes.BGR2HSV);

        // change it to purple

        foreach (var pixel in points)
        {
            int row = pixel.X;
            int col = pixel.Y;
            Vec3b hsvPixel = hsv.At<Vec3b>(row, col);

            double h = 300;
            double s = 100;
            double v = 50;

            s += value;

            if (s < 0)
            {
                s = 0;
            }
            else if (s > 255)
            {
                s = 255;
            }

            hsvPixel.Item0 = (byte)h;
            hsvPixel.Item1 = (byte)s;
            hsvPixel.Item2 = (byte)v;

            hsv.Set<Vec3b>(row, col, hsvPixel);
        }

        Cv2.CvtColor(hsv, hsv, ColorConversionCodes.HSV2BGR);

        return hsv;
    }

    /// <summary>
    /// Masks the original image with the anonymized parts.
    /// </summary>
    /// <param name="img"> The original image.</param>
    /// <param name="eroded"> The eroded image.</param>
    /// <returns> The masked image.</returns>
    internal static Mat MaskOriginal(Mat img, Mat eroded)
    {
        var result = new Mat();
        Cv2.BitwiseAnd(img, img, result, mask: eroded);
        return result;
    }
}