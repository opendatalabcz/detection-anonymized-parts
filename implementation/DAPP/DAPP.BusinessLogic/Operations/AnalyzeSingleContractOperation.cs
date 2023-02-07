namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;
	using DAPP.Models;

	using ImageMagick;

	using OpenCvSharp;

	using System;
	using System.Diagnostics;
	using System.IO;

	public sealed class AnalyzeSingleContractOperation : IAnalyzeSingleContractOperation
	{
		private readonly IContractRepository contractRepository;
		private readonly List<IGetBlackBoundingBoxesOperation> getBlackBoundingBoxesOperations;
		public AnalyzeSingleContractOperation(IContractRepository contractRepository, List<IGetBlackBoundingBoxesOperation> getBlackBoundingBoxesOperations)
		{
			this.contractRepository = contractRepository;
			this.getBlackBoundingBoxesOperations = getBlackBoundingBoxesOperations;
		}
		public AnalyzedContractModel? Execute(int contractId)
		{
			Contract? contract = contractRepository.GetContract(contractId);

			Console.WriteLine(Config.ConsoleDelimeter);
			Console.WriteLine($"Preprocessing {contract.Name}...");
			PreprocessContract(contract);
			Console.WriteLine(Config.ConsoleDelimeter);
			Console.WriteLine($"Analyzing {contract.Name}...");

			//var result = new AnalyzedContractModel();

			AnalyzedContractModel result = new()
			{
				Name = contract.Name,
				ContractId = contractId,
				FilePath = contract.FilePath,
				PagesCount = contract.ContractPages.Count,
				AnonymizedAreaInPercentages = new List<float>(),
			};

			foreach (ContractPage page in contract.ContractPages)
			{

				foreach (IGetBlackBoundingBoxesOperation fc in getBlackBoundingBoxesOperations)
				{

					List<BoundingBoxModel> boundingBoxes = fc.Execute(page);
					(Mat combinedBoundingBoxes, Mat overlay) = OverlayBoundingBoxes(page, boundingBoxes);
					SaveAsImage(overlay, contract, page);
					result.AnonymizedAreaInPercentages.Add(CalculateBlackenedAreaPercentage(combinedBoundingBoxes));
				}
			}
			DeleteTempFiles(contract);
			return result;
		}

		private void DeleteTempFiles(Contract contract)
		{
			foreach (string file in Directory.GetFiles(Config.TestDataFolderPath + "pngs/" + contract.Name))
			{
				File.Delete(file);
			}
			foreach (string file in Directory.GetFiles(Config.TestDataFolderPath + "results/" + contract.Name))
			{
				File.Delete(file);
			}
		}

		private (Mat combinedBoundingBoxes, Mat overlay) OverlayBoundingBoxes(ContractPage page, List<BoundingBoxModel> boundingBoxes)
		{
			Mat src = Cv2.ImRead(page.Path, ImreadModes.Grayscale);
			var color = new Mat(src.Rows, src.Cols, MatType.CV_8UC3, new Scalar(0, 0, 0));
			var result = new Mat();

			foreach (BoundingBoxModel box in boundingBoxes)
			{
				var rect = new Rect(box.X, box.Y, box.Width, box.Height);
				Cv2.Rectangle(color, rect, Scalar.Red, 2);
				if (src.Rows != color.Rows || src.Cols != color.Cols)
				{
					Cv2.Resize(color, src, new Size(src.Cols, src.Rows));
				}
			}

			color = RemoveSmallObjects(color, 8);
			Cv2.CvtColor(src, result, ColorConversionCodes.GRAY2BGR);
			Cv2.AddWeighted(result, 1, color, 1, 0, result);
			//Cv2.ImShow($"Page n. {page.Number} Function used to detect bounding boxes: {fcName}", result);
			_ = Cv2.Threshold(color, color, 1, 255, ThresholdTypes.Binary);
			return (color, result);
		}

		private static float CalculateBlackenedAreaPercentage(Mat color)
		{
			long black = 0;
			long total = color.Width * color.Height;
			for (int x = 0; x < color.Width; x++)
			{
				for (int y = 0; y < color.Height; y++)
				{
					Vec3b px = color.At<Vec3b>(x, y);
					if (px.Item1 + px.Item2 + px.Item0 > 0)
					{
						black++;
					}
				}
			}
			return black / (float)total;
		}

		private static Mat RemoveSmallObjects(Mat image, int size)
		{
			Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(size, size));
			var eroded = new Mat();
			Cv2.Erode(image, eroded, kernel);
			var result = new Mat();
			Cv2.Dilate(eroded, result, kernel);
			return result;
		}
		private void PreprocessContract(Contract contract)
		{
			_ = contract.Extension switch
			{
				".pdf" => SaveAsImages(contract),
				".jpg" => CopyToImagesFolder(contract),
				".jpeg" => CopyToImagesFolder(contract),
				".png" => CopyToImagesFolder(contract),
				_ => throw new UnreachableException(),
			};

		}

		private dynamic CopyToImagesFolder(Contract contract)
		{
			File.Copy(contract.FilePath, Config.ContractsFolderPath + "\\.." + "\\pngs\\" + contract.Name + "\\" + contract.Name + contract.Extension);
			return 0;
		}

		private void SaveAsImage(Mat overlay, Contract contract, ContractPage contractPage)
		{
			_ = Path.Combine("results", contract.Name);
			string resultsFolderPath = Path.GetDirectoryName(contract.FilePath) + "\\.." + "\\results";
			if (!Directory.Exists(resultsFolderPath))
			{
				_ = Directory.CreateDirectory(resultsFolderPath);
			}
			string pdfDir = Path.Combine(resultsFolderPath, contract.Name);
			if (!Directory.Exists(pdfDir))
			{
				_ = Directory.CreateDirectory(pdfDir);
			}
			Console.WriteLine($"Saving {contract.Name} results...");
			Console.WriteLine($"Page n. {contractPage.Number}...");
			_ = overlay.SaveImage(Path.Combine(pdfDir, $"page_{contractPage.Number}.png"));
		}

		private dynamic SaveAsImages(Contract contract)
		{
			// Load pdf // Get list of individual pages as Image object
			// For each page change color of all text to white;
			_ = Path.Combine("pngs", contract.Name);
			string pngFolderPath = Path.GetDirectoryName(contract.FilePath) + "\\.." + "\\pngs";
			if (!Directory.Exists(pngFolderPath))
			{
				_ = Directory.CreateDirectory(pngFolderPath);
			}

			var settings = new MagickReadSettings
			{
				Density = new Density(Config.Density.x, Config.Density.y, DensityUnit.PixelsPerCentimeter),
			};

			using var images = new MagickImageCollection(contract.FilePath, settings);

			string pdfDir = Path.Combine(pngFolderPath, contract.Name);
			if (!Directory.Exists(pdfDir))
			{
				_ = Directory.CreateDirectory(pdfDir);
			}

			Console.WriteLine($"Saving {contract.Name} to pngs...");
			Console.WriteLine($"Total pages:{images.Count()}");
			// iterate over all images
			int page = 1;
			foreach (MagickImage image in images)
			{
				Console.WriteLine($"Saving page number {page} to: page_{page}.png ...");
				contract.ContractPages.Add(new() { Path = Path.Combine(pdfDir, $"page_{page}.png"), Number = page });
				image.Write(Path.Combine(pdfDir, $"page_{page}.png"));
				page++;
			}

			return 0;
		}
	}
}
