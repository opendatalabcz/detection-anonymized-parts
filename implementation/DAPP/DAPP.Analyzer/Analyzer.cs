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

		static string rootPath = "..\\..\\..\\..\\Results\\";

		public ErrorOr<AnalyzedPage> AnalyzePage(MagickImage image, ContractPage page)
        {
            // Validate if image is in correct format
            var validation = ValidateImage(image);
            if (validation is Error v)
                return v;

            // Create temporary folder for storing computational data (such as preprocessed image etc)
            EnsureDirectoryExists(page);

            // Create Mat object in Cv2
            Mat img = ToMat(image);

            // Save original
            Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\original.jpg", img);

			Mat result = GetAnonymizedParts(img, erodeValue: 9, dilateValue: 3);

            Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\result.jpg", result);
			Cv2.ImWrite($"{rootPath}\\{page.Contract.Name}\\{page.Id}\\result_masked.jpg", MaskOriginal(img,result));

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

        private Mat MaskOriginal(Mat img, Mat eroded)
        {
			var result = new Mat();
			Cv2.BitwiseAnd(img, img, result, mask:eroded);
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

        /// <summary>
        /// Determines the type of anonymization used on the image
        /// </summary>
        /// <param name="img"> The image to be analyzed </param>
        /// <param name="isColorful"> Whether the image contains any colorful pixels </param>
        /// <returns> The type of anonymization used, The areas that were anonymized  </returns>
  //      private (AnonymizationTypeEnum, float anonymizedArea) FindAnonymized(Mat img, List<Point> coloredPixels)
		//{
		//	List<Rect> boundingBoxes = new List<Rect>();
		//	var anonymizationType = AnonymizationType.None;
		//	if (coloredPixels.Any())
		//	{
		//		// Create bounding boxes around the colored pixels
		//		boundingBoxes = FindBoundingBoxesColored(img, coloredPixels);
		//		anonymizationType = AnonymizationType.ColoredSticker;
		//	}

		//	else
		//	{
		//		// Create bounding boxes around black rectangles (black pixels)
		//		boundingBoxes = FindBoundingBoxesBlack(img);
		//		// Remove small bounding boxes and bounding boxes that are too large (98% of the image)
		//		anonymizationType = AnonymizationType.BlackRectangle;
		//	}
		//	boundingBoxes = boundingBoxes.Where(
		//		b => b.Width * b.Height > 200 &&
		//		b.Width > 8 && b.Height > 8 &&
		//		b.Width * b.Height < img.Width * img.Height * 0.98).ToList();

		//	if (boundingBoxes.Count == 0)
		//	{
		//		anonymizationType = AnonymizationType.None;
		//	}
		//	// Render the bounding boxes on the copy of an image
		//	Mat imgCopy = img.Clone();
		//	foreach (var box in boundingBoxes)
		//	{
		//		Cv2.Rectangle(imgCopy, box, Scalar.Red, 2);
		//	}


		//	// Calculate the area that was anonymized using the bounding boxes
		//	var anonymizedArea = CalculateAnonymizedArea(img, boundingBoxes);

  //          // Display 
  //          //Cv2.ImShow("Original", img);
  //          //Cv2.ImShow($"Anonymization type: {anonymizationType}, Anonymized area: {anonymizedArea}", imgCopy);
		//	//Cv2.WaitKey(0);


		//	return (anonymizationType, anonymizedArea);
		//}

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
			if (blackPixels == 0 && blackPixelsAfterAccountingForMasking == 0) return 0;
			return blackPixelsAfterAccountingForMasking / (blackPixels + blackPixelsAfterAccountingForMasking);
        }


		//private List<Rect> FindBoundingBoxesBlack(Mat img)
		//{
		//	var boundingBoxes = new List<Rect>();

		//	// Create a copy of the image
		//	var imgCopy = img.Clone();

		//	// Convert the image to grayscale
		//	Cv2.CvtColor(imgCopy, imgCopy, ColorConversionCodes.BGR2GRAY);

		//	// Find the contours of the image
		//	var contours = new Point[][] { };
		//	var hierarchy = new HierarchyIndex[] { };
		//	Cv2.FindContours(imgCopy, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

		//	// Loop over the contours
		//	foreach (var contour in contours)
		//	{
		//		// Calculate the bounding box of the contour
		//		var boundingBox = Cv2.BoundingRect(contour);
		//		// Add the bounding box to the list
		//		boundingBoxes.Add(boundingBox);
		//	}
		//	// Filter bounding boxes that have more than 20% of white inside
		//	// Convert to grayscale, threshold to 2 colors and count the percentage
		//	var filteredBoundingBoxes = new List<Rect>();
		//	foreach (var box in boundingBoxes)
		//	{
		//		// Create a mat for the bounding box
		//		var boxMat = img.SubMat(box);
		//		// Convert to grayscale
		//		Cv2.CvtColor(boxMat, boxMat, ColorConversionCodes.BGR2GRAY);
		//		// Threshold to 2 colors
		//		Cv2.Threshold(boxMat, boxMat, 0, 255, ThresholdTypes.Otsu);
		//		// Count the white pixels in the bounding box
		//		var whitePixels = boxMat.CountNonZero();
		//		// Calculate the percentage of white pixels
		//		var percentage = whitePixels / (float)(box.Width * box.Height);
		//		// If the percentage is less than 10%, add the bounding box to the list
		//		if (percentage < 0.30)
		//			filteredBoundingBoxes.Add(box);
		//	}
		//	// Return the filtered bounding boxes
		//	return filteredBoundingBoxes;

		//}
		//private List<Rect> FindBoundingBoxesColored(Mat img, List<Point> coloredPixels)
		//{
		//	// Create new image, where every pixel is 0 (black) except for the colored pixels which are 255 (white)

		//	Mat mask = new Mat(img.Rows, img.Cols, MatType.CV_8UC1, new Scalar(0));
		//	foreach (var pixel in coloredPixels)
		//	{
		//		mask.Set(pixel.X, pixel.Y, 255);
		//	}

		//	// Find the contours of the colored pixels

		//	Point[][] contours;
		//	var hierarchy = new HierarchyIndex[0];
		//	Cv2.FindContours(mask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

		//	// Create bounding boxes around the contours

		//	List<Rect> boundingBoxes = new List<Rect>();
		//	foreach (var contour in contours)
		//	{
		//		boundingBoxes.Add(Cv2.BoundingRect(contour));
		//	}

		//	// Filter bounding boxes that have more than 5% of black inside
		//	// Convert to grayscale, threshold to 2 colors and count the percentage
		//	Mat imgGray = new Mat();
		//	Cv2.CvtColor(img, imgGray, ColorConversionCodes.BGR2GRAY);
		//	Mat imgThresh = new Mat();
		//	Cv2.Threshold(imgGray, imgThresh, 127, 255, ThresholdTypes.Binary);
		//	List<Rect> filteredBoundingBoxes = new List<Rect>();
		//	foreach (var box in boundingBoxes)
		//	{
		//		var subMat = imgThresh.SubMat(box);
		//		var blackPixels = subMat.CountNonZero();
		//		var percentage = blackPixels / (float)(box.Width * box.Height);
		//		if (percentage > 0.95)
		//			filteredBoundingBoxes.Add(box);
		//	}
		//	return filteredBoundingBoxes;
		//}

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
				return new List<Point>();
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

        private Mat AdaptiveThreshold(Mat src)
        {
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            Mat result = new Mat();
            Cv2.AdaptiveThreshold(gray, result, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);

            return result;
        }

        private Mat Threshold(Mat src, int val)
        {
            Mat gray = new Mat();
			if (src.Channels() == 1) gray = src.Clone();
			else
			{
				Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
			}
            Mat result = new Mat();
            Cv2.Threshold(gray, result, val, 255, ThresholdTypes. Otsu);

            return result;
        }

        private Mat IncreaseContrast(Mat src, double alpha, double beta)
        {
            Mat result = new Mat();
            src.ConvertTo(result, -1, alpha, beta);
            return result;
        }

        private Mat IncreaseSaturation(Mat src, List<Point> points, double value)
        {
			if (points.Count == 0) return src;
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

