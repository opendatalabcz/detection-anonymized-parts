namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Models;

	using OpenCvSharp;

	using System;
	using System.Diagnostics;

	public sealed class AnalyzeSingleContractOperation : IAnalyzeSingleContractOperation
	{
		private readonly IContractRepository contractRepository;

		public AnalyzeSingleContractOperation(IContractRepository contractRepository)
		{
			this.contractRepository = contractRepository;
		}

		public void Execute(int contractId)
		{
			Console.WriteLine($"Analyzing contract {contractId}");
			Entities.Contract contract = contractRepository.GetContract(contractId);
			if (contract == null)
			{
				throw new ArgumentException($"Contract with id {contractId} does not exist");
			}

			// generate png from pdf pages and save to disk to separate folder
			ProcessStartInfo processStartInfo = new()
			{
				FileName = "pdftoppm.exe",
				Arguments = $"-png {contract.FilePath} {contract.FilePath}_page",
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
			var p = Process.Start(processStartInfo);
			p.WaitForExit();

			// for each png file in folder
			// 1. load png file
			// 2. convert to grayscale
			// 3. apply threshold
			// 4. apply canny edge detection
			// 5. find contours
			// 6. find bounding boxes
			// 7. save bounding boxes to AnalyzedContractModel
			// 8. save AnalyzedContractModel to contractRepository

			// 1. load png files

			Mat image = Cv2.ImRead($"{contract.FilePath}_page-1.png");
			// 2. convert to grayscale
			Mat grayImage = new();
			Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
			// 3. apply threshold
			Mat thresholdImage = new();
			_ = Cv2.Threshold(grayImage, thresholdImage, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
			// 4. apply canny edge detection
			Mat cannyImage = new();
			Cv2.Canny(thresholdImage, cannyImage, 100, 200);

			// 5. find contours
			Cv2.FindContours(cannyImage, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
			// 6. find bounding boxes
			List<Rect> boundingBoxes = new();
			foreach (Point[] contour in contours)
			{
				boundingBoxes.Add(Cv2.BoundingRect(contour));
			}
			// 7. save bounding boxes to AnalyzedContractModel
			AnalyzedContractModel analyzedContractModel = new()
			{
				ContractId = contractId,
			};
			foreach (Rect boundingBox in boundingBoxes)
			{
				analyzedContractModel.BoundingBoxes.Add(new BoundingBoxModel
				{
					X = boundingBox.X,
					Y = boundingBox.Y,
					Width = boundingBox.Width,
					Height = boundingBox.Height
				});
			}

			contractRepository.SaveAnalyzedContract(analyzedContractModel);

		}
	}
}