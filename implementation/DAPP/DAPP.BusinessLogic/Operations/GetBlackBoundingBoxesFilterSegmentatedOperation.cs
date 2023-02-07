namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.Entities;
	using DAPP.Models;

	using OpenCvSharp;

	using System.Collections.Generic;

	public sealed class GetBlackBoundingBoxesSegmentatedFilterOperation : IGetBlackBoundingBoxesOperation
	{

		public string Name { get; } = "Performs segmentation of an image";

		public List<BoundingBoxModel> Execute(ContractPage page)
		{
			List<BoundingBoxModel> res = new();
			Mat src = Cv2.ImRead(page.Path, ImreadModes.Grayscale);
			_ = Cv2.Threshold(src, src, 170, 255, ThresholdTypes.Binary);
			List<(Mat, int, int)> smallerImages = SegmentateImage(RemoveBorder(src, 20), Config.SegmentsCount);
			foreach ((Mat, int, int) simg in smallerImages)
			{
				if (AllPixelsBlack(simg.Item1))
				{
					res.Add(new(
						 simg.Item3 * simg.Item1.Width,
						 simg.Item2 * simg.Item1.Height, simg.Item1.Width, simg.Item1.Height));
				}
			}
			return res;
		}

		private static Mat RemoveBorder(Mat image, int borderWidth)
		{
			int rows = image.Rows;
			int cols = image.Cols;

			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					if (x < borderWidth || x >= cols - borderWidth || y < borderWidth || y >= rows - borderWidth)
					{
						image.Set<Vec3b>(y, x, new Vec3b(255, 255, 255));
					}
				}
			}
			return image;
		}
		private bool AllPixelsBlack(Mat image)
		{
			for (int i = 0; i < image.Rows; i++)
			{
				for (int j = 0; j < image.Cols; j++)
				{
					if (image.At<byte>(i, j) != 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		private static List<(Mat, int, int)> SegmentateImage(Mat image, int power)
		{

			List<(Mat, int, int)> segments = new();
			int rows = image.Rows;
			int cols = image.Cols;
			int a = (int)Math.Sqrt(power);
			int b = (int)Math.Ceiling((double)power / a);

			int segmentRows = (int)Math.Ceiling((double)image.Rows / a);
			int segmentCols = (int)Math.Ceiling((double)image.Cols / b);

			Mat white = Mat.Ones(segmentRows, segmentCols, MatType.CV_8UC3);
			white *= 255;

			for (int y = 0; y < a; y++)
			{
				for (int x = 0; x < b; x++)
				{
					int x1 = x * segmentCols;
					int y1 = y * segmentRows;
					int x2 = Math.Min((x + 1) * segmentCols, cols);
					int y2 = Math.Min((y + 1) * segmentRows, rows);

					if (x1 >= cols || y1 >= rows)
					{
						segments.Add((white.Clone(), y, x));
						continue;
					}

					var roi = new Rect(x1, y1, x2 - x1, y2 - y1);
					var segment = new Mat(image, roi);

					if (x2 - x1 < segmentCols || y2 - y1 < segmentRows)
					{
						var resized = new Mat();
						Cv2.Resize(segment, resized, new Size(segmentCols, segmentRows), 0, 0, InterpolationFlags.Area);
						var padded = new Mat(white.Rows, white.Cols, MatType.CV_8UC3);
						_ = padded.SetTo(new Scalar(255, 255, 255));
						resized.CopyTo(padded.SubMat(new Rect(0, 0, resized.Cols, resized.Rows)));
						segments.Add((padded, y, x));
					}
					else
					{
						segments.Add((segment, y, x));
					}
				}
			}

			return segments;
		}
	}
}
