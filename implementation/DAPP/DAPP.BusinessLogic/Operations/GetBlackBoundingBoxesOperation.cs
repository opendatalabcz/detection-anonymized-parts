namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.Entities;
	using DAPP.Models;

	using OpenCvSharp;

	public sealed class GetBlackBoundingBoxesOperation : IGetBlackBoundingBoxesOperation
	{
		public string Name { get; } = "Tresholding image and Finding contours";

		public List<BoundingBoxModel> Execute(ContractPage page)
		{
			List<BoundingBoxModel> res = new();
			Mat src = Cv2.ImRead(page.Path, ImreadModes.Grayscale);
			var bin = new Mat();
			_ = Cv2.Threshold(src, bin, 0, 255, ThresholdTypes.BinaryInv);

			Cv2.FindContours(bin, out Point[][] contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

			_ = new Mat(src.Rows, src.Cols, MatType.CV_8UC3, new Scalar(0, 0, 0));
			for (int i = 0; i < contours.Length; i++)
			{
				Rect rect = Cv2.BoundingRect(contours[i]);
				if (rect.Width < 10 || rect.Height < 10)
					continue;
				res.Add(new(rect.X, rect.Y, rect.Width, rect.Height));
			}
			return res;
		}
	}
}
