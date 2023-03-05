namespace DAPP.Analyzer
{
	using System;

	using DAPP.Application.Interfaces;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities;
	using DAPP.Domain.Aggregates.ContractAggregate.Entities;
	using DAPP.Domain.Common;

	using ErrorOr;

	using ImageMagick;

	using OpenCvSharp;

	public class Analyzer : IAnalyzer
	{

		public ErrorOr<AnalyzedPage> AnalyzePage(MagickImage image, ContractPage page)
		{
			// Validate if image is in correct format
			var validation = ValidateImage(image);
			if (validation is Error v)
				return v;

			// Create temporary folder for storing computational data (such as preprocessed image etc)
			if (!Directory.Exists("temp"))
			{
				Console.WriteLine("Creating temp folder...");
				Directory.CreateDirectory("temp");
			}

			// Create Mat object in Cv2
			Mat img = ToMat(image);

			// Determine whether there are any color pixels (might be colorful sticker used as anonymization)
			var isColorful = IsColorful(img);

			// Determine Scan type (whether its digital or scanned)
			var scanType = DetermineScanType(img);
			// Determine Type of Anonymization, if any
			var anonymizationType = DetermineAnonymizationType(img, isColorful.scanType);

			// Calculate the area that was anonymized based on anonymization type

			return new AnalyzedPage()
			{
				IsGrayscale = !isColorful,
				AnonymizationType = anonymizationType,
				ContractPage = page,
			};
		}

		private AnonymizationTypeEnum DetermineAnonymizationType(Mat img, bool isColorful, ScanTypeEnum scanType)
		{

			// Try to detect colorful areas (most likely used as anonymization)
			if (isColorful)
			{
				// try to find colorful areas
				// if found, return AnonymizationTypeEnum.Colorful

			}
			// Try to detect black rectangles (most likely used as anonymization)
			// Try to detect grainy areas (less likely used as anonymization)
			bool foundBlackRectangles = TryToFindBlackRectangles(img, scanType);
			if (foundBlackRectangles)
			{
				return AnonymizationTypeEnum.BlackRectangle;
			}
			return AnonymizationTypeEnum.None;
		}

		private bool TryToFindBlackRectangles(Mat img, ScanTypeEnum scanType)
		{
			throw new NotImplementedException();
		}

		private ScanTypeEnum DetermineScanType(Mat img)
		{
			// Convert the image to grayscale
			Mat gray = new Mat();
			Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

			// Apply Gaussian blur to the image
			Mat blur = new Mat();
			Cv2.GaussianBlur(gray, blur, new Size(3, 3), 0);

			// Apply Canny edge detection
			Mat canny = new Mat();
			Cv2.Canny(blur, canny, 50, 150);

			// Find contours
			Point[][] contours;
			HierarchyIndex[] hierarchy;
			Cv2.FindContours(canny, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

			// Find the largest contour
			double maxArea = 0;
			int maxAreaIndex = 0;
			for (int i = 0; i < contours.Length; i++)
			{
				double area = Cv2.ContourArea(contours[i]);
				if (area > maxArea)
				{
					maxArea = area;
					maxAreaIndex = i;
				}
			}

			// Create a mask of the largest contour
			Mat mask = Mat.Zeros(img.Size(), MatType.CV_8UC1);
			Cv2.DrawContours(mask, contours, maxAreaIndex, Scalar.All(255), -1);

			// Calculate the percentage of the largest contour
			double percentage = (double)Cv2.CountNonZero(mask) / (img.Rows * img.Cols);

			// If the percentage is greater than 0.5, then the image is a digital image
			if (percentage > 0.5)
			{
				return ScanType.Digital;
			}
			else
			{
				return ScanType.Paperscan;
			}
		}

		private bool IsColorful(Mat img)
		{
			// iterate through each pixel and check if it is colorful
			for (int i = 0; i < img.Rows; i++)
			{
				for (int j = 0; j < img.Cols; j++)
				{
					Vec3b pixel = img.At<Vec3b>(i, j);
					if (pixel.Item0 != pixel.Item1 || pixel.Item1 != pixel.Item2)
					{
						return true;
					}
				}
			}
			return false;
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
	}
}

