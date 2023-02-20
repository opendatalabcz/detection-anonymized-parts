namespace DAPP.BusinessLogic.Operations
{
	using System.Collections.Generic;

	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.Entities;
	using DAPP.Models;

	using OpenCvSharp;

	public sealed class GetBlackBoundingBoxesQuadtreeOperation : IGetBlackBoundingBoxesOperation
	{

		public string Name { get; } = "Performs segmentation of an image using quadtree";

		public List<BoundingBoxModel> Execute(ContractPage page)
		{
			var res = new List<BoundingBoxModel>();

			Mat src = Cv2.ImRead(page.Path, ImreadModes.Grayscale);
			_ = Cv2.Threshold(src, src, 170, 255, ThresholdTypes.Binary);

		}
	}
}
