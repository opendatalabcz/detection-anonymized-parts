using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Unit.Tests")]
namespace DAPPAnalyzer.Services;
public class PDFAnalyzer
{
    public static async Task<AnalyzedResult> AnalyzeAsync(DappPDF pdf, bool returnImages = false)
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
                if (returnImages)
                {
                    originalImages[i] = page.ToBytes(".jpg");
                    var masked = MaskOriginal(page, anonymizedParts);
                    anonymizedImages[i] = masked.ToBytes(".jpg");
                }
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

    internal static (Mat anonymizedParts, bool containsAnonymizedData, float anonymizedPercentage) AnalyzePage(Mat page)
    {
        var anonymizedParts = GetAnonymizedParts(page);
        var anonymizedPercentage = (float)((page.Rows * page.Cols) - anonymizedParts.CountNonZero()) / (page.Rows * page.Cols);
        var containsAnonymizedData = anonymizedPercentage > 0.01;
        return (anonymizedParts, containsAnonymizedData, anonymizedPercentage);
    }

    internal static Mat GetAnonymizedParts(Mat img, int erodeValue = 8, int dilateValue = 4)
    {

        var coloredPixels = ColoredPixels(img);
        // Increase their saturation 
        var imgSaturatedColors = IncreaseSaturation(img, coloredPixels, 100);

        // Create structuring element
        Mat se = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
        var dilated = Dilate(imgSaturatedColors, se);
        //Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\dilated.jpg", dilated);

        var dilated_threshold = Threshold(dilated, 20);
        var dilated2 = Dilate(dilated_threshold, se);
        //Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\dilated_threshold_dilated.jpg", dilated2);


        var result = dilated2;
        for (int i = 0; i < erodeValue; i++)
        {
            result = Erode(result, se);
        }
        for (int i = 0; i < dilateValue; i++)
        {
            result = Dilate(result, se);
        }

        // Remove black parts that are directly touching borders

        return result;
    }


    internal static List<Point> ColoredPixels(Mat img)
    {
        List<Point> coloredPixels = new List<Point>();
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
    internal static Mat Dilate(Mat src, Mat pattern)
    {
        Mat result = new Mat();
        Cv2.Dilate(src, result, pattern);
        return result;
    }

    internal static Mat Erode(Mat src, Mat pattern)
    {
        Mat result = new Mat();
        Cv2.Erode(src, result, pattern);
        return result;
    }

    internal static Mat Threshold(Mat src, int val)
    {
        Mat gray = new Mat();
        if (src.Channels() == 1)
        {
            gray = src.Clone();
        }
        else
        {
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
        }
        Mat result = new Mat();
        Cv2.Threshold(gray, result, val, 255, ThresholdTypes.Otsu);

        return result;
    }

    internal static Mat IncreaseSaturation(Mat src, List<Point> points, double value)
    {
        if (points.Count == 0)
        {
            return src;
        }

        Mat hsv = new Mat();
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

            s = s + value;

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

    internal static Mat MaskOriginal(Mat img, Mat eroded)
    {
        var result = new Mat();
        Cv2.BitwiseAnd(img, img, result, mask: eroded);
        return result;
    }
}