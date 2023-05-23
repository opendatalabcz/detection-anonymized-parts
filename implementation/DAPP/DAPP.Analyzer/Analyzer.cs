namespace DAPP.Analyzer
{
    using System;
    using DAPP.Application.Interfaces;
    using DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities;
    using DAPP.Domain.Aggregates.ContractAggregate.Entities;
    using ErrorOr;

    using ImageMagick;

    using OpenCvSharp;

    public class Analyzer : IAnalyzer
    {

        static readonly string rootPath = "..\\..\\..\\..\\Results\\";

        public ErrorOr<AnalyzedPage> AnalyzePage(MagickImage image, ContractPage page)
        {
            // Validate if image is in correct format
            var validation = ValidateImage(image);
            if (validation is Error v)
            {
                return v;
            }

            // Create temporary folder for storing computational data (such as preprocessed image etc)
            EnsureDirectoryExists(page);

            // Create Mat object in Cv2
            Mat img = ToMat(image);

            // Save original
            Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\original.jpg", img);

            Mat result = GetAnonymizedParts(img, erodeValue: 9, dilateValue: 3);

            Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\result.jpg", result);
            Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\result_masked.jpg", MaskOriginal(img, result));

            var fl = (float)CalculateAnonymizedArea(img, result);
            return new AnalyzedPage()
            {
                ContractPage = page,
                AnonymizationPercentage = fl,
            };
        }

        private Mat GetAnonymizedParts(Mat img, int erodeValue = 8, int dilateValue = 4)
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

        private static Mat MaskOriginal(Mat img, Mat eroded)
        {
            var result = new Mat();
            Cv2.BitwiseAnd(img, img, result, mask: eroded);
            return result;
        }

        private static void EnsureDirectoryExists(ContractPage page)
        {
            if (!Directory.Exists(rootPath))
            {
                Console.WriteLine("Creating temp folder...");
                Directory.CreateDirectory(rootPath);
            }
            if (!Directory.Exists($"{rootPath}\\{page.Contract.Name} \\"))
            {
                Directory.CreateDirectory($"{rootPath}\\{page.Contract.Name}");
            }

            if (!Directory.Exists($"{rootPath}\\{page.Contract.Name}\\{page.Id}"))
            {
                Directory.CreateDirectory($"{rootPath}\\{page.Contract.Name}\\{page.Id}");
            }
        }

        private double CalculateAnonymizedArea(Mat img, Mat mask, double maskPercentageAccounting = 0.08)
        {
            // count mask
            long blackPixels = 0;
            Mat threshold = Threshold(img, 20);

            for (int i = 0; i < threshold.Rows; i++)
            {
                for (int j = 0; j < threshold.Cols; j++)
                {
                    var px = threshold.At<Vec3b>(i, j);
                    if (px[0] == px[1] && px[1] == px[2] && px[2] == 0)
                    {
                        blackPixels++;
                    }
                }
            }

            long blackPixelsMask = 0;
            for (int i = 0; i < mask.Rows; i++)
            {
                for (int j = 0; j < mask.Cols; j++)
                {
                    var px = mask.At<Vec3b>(i, j);
                    if (px[0] == px[1] && px[1] == px[2] && px[2] == 0)
                    {
                        blackPixelsMask++;
                    }
                }
            }

            var blackPixelsAfterAccountingForMasking = blackPixelsMask * maskPercentageAccounting;
            if (blackPixels == 0 && blackPixelsAfterAccountingForMasking == 0)
            {
                return 0;
            }

            return blackPixelsAfterAccountingForMasking / (blackPixels + blackPixelsAfterAccountingForMasking);
        }

        private List<Point> ColoredPixels(Mat img)
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

        private Mat ToMat(MagickImage image)
        {
            // Convert the MagickImage image to a byte array
            byte[] bytes = image.ToByteArray(MagickFormat.Bmp);

            // Create a Mat object using the byte array
            Mat mat = Cv2.ImDecode(bytes, ImreadModes.Unchanged);
            return mat;
        }

        private static Error? ValidateImage(MagickImage image)
        {
            if (image == null)
            {
                return Errors.Analyzer.Validation.Image.IsNull;
            }

            if (image.Width == 0 || image.Height == 0)
            {
                return Errors.Analyzer.Validation.Image.InvalidDimension;
            }

            return null;
        }

        private Mat Dilate(Mat src, Mat pattern)
        {
            Mat result = new Mat();
            Cv2.Dilate(src, result, pattern);
            return result;
        }

        private Mat Erode(Mat src, Mat pattern)
        {
            Mat result = new Mat();
            Cv2.Erode(src, result, pattern);
            return result;
        }

        private Mat Threshold(Mat src, int val)
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

        private Mat IncreaseSaturation(Mat src, List<Point> points, double value)
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
    }
}

