namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.Entities;
	using DAPP.Models;

	using OpenCvSharp;

	using System.Collections.Generic;

	public sealed class GetBlackBoundingBoxesHighPassFilterOperation : IGetBlackBoundingBoxesOperation
	{

		public string Name { get; } = "Performs High pass filter using gaussian blur";

		public List<BoundingBoxModel> Execute(ContractPage page)
		{
			List<BoundingBoxModel> res = new();
			Mat src = Cv2.ImRead(page.Path, ImreadModes.Grayscale);

			src = HighPassFilter(src);

			Cv2.Canny(src, src, 199, 220);

			Cv2.FindContours(src, out Point[][] contours, out _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

			_ = new Mat(src.Rows, src.Cols, MatType.CV_8UC3, new Scalar(0, 0, 0));
			for (int i = 0; i < contours.Length; i++)
			{
				Rect rect = Cv2.BoundingRect(contours[i]);
				if (rect.Width < 20 || rect.Height < 20)
					continue;
				res.Add(new(rect.X, rect.Y, rect.Width, rect.Height));
			}
			return res;
		}

		private Mat HighPassFilter(Mat source)
		{
			var blur = new Mat();
			Cv2.GaussianBlur(source, blur, new Size(Config.KSize.x, Config.KSize.y), 0);
			MatExpr filtered = source - blur;
			var result = new Mat();
			Cv2.BitwiseNot(filtered, result);
			return result;
		}
	}
}
