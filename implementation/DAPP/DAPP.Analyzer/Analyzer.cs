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
			var coloredPixels = ColoredPixels(img);

			// Determine Type of Anonymization, if any
			var anonymization = FindAnonymized(img, coloredPixels);

			// Calculate the area that was anonymized based on anonymization type

			return new AnalyzedPage()
			{
				IsGrayscale = coloredPixels.Any(),
				AnonymizationType = anonymization.Item1,
				ContractPage = page,
				AnonymizationPercentage = anonymization.Item2,
			};
		}

		/// <summary>
		/// Determines the type of anonymization used on the image
		/// </summary>
		/// <param name="img"> The image to be analyzed </param>
		/// <param name="isColorful"> Whether the image contains any colorful pixels </param>
		/// <returns> The type of anonymization used, The areas that were anonymized  </returns>
		private (AnonymizationTypeEnum, float anonymizedArea) FindAnonymized(Mat img, List<Point> coloredPixels)
		{
			List<Rect> boundingBoxes = new List<Rect>();
			var anonymizationType = AnonymizationType.None;
			if(coloredPixels.Any())
			{
				// Create bounding boxes around the colored pixels
				boundingBoxes = FindBoundingBoxesColored(img, coloredPixels);
                anonymizationType = AnonymizationType.ColoredSticker;
			}

			else
			{
				// Create bounding boxes around black rectangles (black pixels)
				boundingBoxes = FindBoundingBoxesBlack(img);
				// Remove small bounding boxes and bounding boxes that are too large (98% of the image)
				anonymizationType = AnonymizationType.BlackRectangle;
			}
			boundingBoxes = boundingBoxes.Where(
				b => b.Width * b.Height > 200 && 
				b.Width > 8 && b.Height > 8 &&
				b.Width * b.Height < img.Width * img.Height * 0.98).ToList();

			if (boundingBoxes.Count == 0)
			{
				anonymizationType = AnonymizationType.None;
			}
            // Render the bounding boxes on the copy of an image
            Mat imgCopy = img.Clone();
			foreach (var box in boundingBoxes)
			{
				Cv2.Rectangle(imgCopy, box, Scalar.Red, 2);
			}


            // Calculate the area that was anonymized using the bounding boxes
            var anonymizedArea = CalculateAnonymizedArea(img, boundingBoxes);

            Cv2.ImShow("Original", img);
			// Display 
			Cv2.ImShow($"Anonymization type: {anonymizationType}, Anonymized area: {anonymizedArea}", imgCopy);
			Cv2.WaitKey(0);


			return (anonymizationType, anonymizedArea);
		}

		private float CalculateAnonymizedArea(Mat img, List<Rect> boundingBoxes)
		{
			float area = 0;
			foreach (var box in boundingBoxes)
				area += box.Width * box.Height;
			return area / (img.Width * img.Height);
		}


        private List<Rect> FindBoundingBoxesBlack(Mat img)
		{
			var boundingBoxes = new List<Rect>();
			
			// Create a copy of the image
			var imgCopy = img.Clone();

			// Convert the image to grayscale
			Cv2.CvtColor(imgCopy, imgCopy, ColorConversionCodes.BGR2GRAY);
			
			// Find the contours of the image
            var contours = new Point[][] { };
			var hierarchy = new HierarchyIndex[] { };
			Cv2.FindContours(imgCopy, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
			
			// Loop over the contours
			foreach (var contour in contours)
			{
                // Calculate the bounding box of the contour
                var boundingBox = Cv2.BoundingRect(contour);
                // Add the bounding box to the list
                boundingBoxes.Add(boundingBox);
            }
            // Filter bounding boxes that have more than 20% of white inside
            // Convert to grayscale, threshold to 2 colors and count the percentage
			var filteredBoundingBoxes = new List<Rect>();
            foreach (var box in boundingBoxes)
            {
                // Create a mat for the bounding box
                var boxMat = img.SubMat(box);
                // Convert to grayscale
                Cv2.CvtColor(boxMat, boxMat, ColorConversionCodes.BGR2GRAY);
                // Threshold to 2 colors
                Cv2.Threshold(boxMat, boxMat, 0, 255, ThresholdTypes.Otsu);
                // Count the white pixels in the bounding box
                var whitePixels = boxMat.CountNonZero();
                // Calculate the percentage of white pixels
                var percentage = whitePixels / (float)(box.Width * box.Height);
                // If the percentage is less than 10%, add the bounding box to the list
                if (percentage < 0.30)
                    filteredBoundingBoxes.Add(box);
            }
            // Return the filtered bounding boxes
            return filteredBoundingBoxes;
			
		}
       private List<Rect> FindBoundingBoxesColored(Mat img, List<Point> coloredPixels)
        {
			// Create new image, where every pixel is 0 (black) except for the colored pixels which are 255 (white)
		
			Mat mask = new Mat(img.Rows, img.Cols, MatType.CV_8UC1, new Scalar(0));
			foreach (var pixel in coloredPixels)
			{
                mask.Set(pixel.X, pixel.Y, 255);
            }

			// Find the contours of the colored pixels

			Point[][] contours;
			var hierarchy = new HierarchyIndex[0];
			Cv2.FindContours(mask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

			// Create bounding boxes around the contours

			List<Rect> boundingBoxes = new List<Rect>();
			foreach (var contour in contours)
			{
				boundingBoxes.Add(Cv2.BoundingRect(contour));
			}

			// Filter bounding boxes that have more than 5% of black inside
			// Convert to grayscale, threshold to 2 colors and count the percentage
			Mat imgGray = new Mat();
			Cv2.CvtColor(img, imgGray, ColorConversionCodes.BGR2GRAY);
			Mat imgThresh = new Mat();
			Cv2.Threshold(imgGray, imgThresh, 127, 255, ThresholdTypes.Binary);
			List<Rect> filteredBoundingBoxes = new List<Rect>();
			foreach (var box in boundingBoxes)
			{
                var subMat = imgThresh.SubMat(box);
                var blackPixels = subMat.CountNonZero();
                var percentage = blackPixels / (float)(box.Width * box.Height);
                if (percentage > 0.95)
                    filteredBoundingBoxes.Add(box);
            }
			return filteredBoundingBoxes;
        }

        private List<Point> ColoredPixels(Mat img)
        {
			List<Point> coloredPixels = new List<Point>();
            // iterate through each pixel and check if it is colorful
			for (int i = 0; i < img.Rows; i++)
			{
				for (int j = 0; j < img.Cols; j++)
				{
					if( 
						Math.Abs(img.At<Vec3b>(i, j)[0] - img.At<Vec3b>(i, j)[1]) > 35 
					||  Math.Abs(img.At<Vec3b>(i, j)[0] - img.At<Vec3b>(i, j)[2]) > 35 
					||  Math.Abs(img.At<Vec3b>(i, j)[1] - img.At<Vec3b>(i, j)[2]) > 35)
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
	}
}

