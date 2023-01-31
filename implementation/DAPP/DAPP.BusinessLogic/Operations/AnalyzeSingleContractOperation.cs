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

	public sealed class AnalyzeSingleContractOperation : IAnalyzeSingleContractOperation
	{
		private readonly IContractRepository contractRepository;
		public AnalyzeSingleContractOperation(IContractRepository contractRepository)
		{
			this.contractRepository = contractRepository;
		}
		public AnalyzedContractModel? Execute(int contractId)
		{
			Contract? contract = contractRepository.GetContract(contractId);

			Console.WriteLine(Config.ConsoleDelimeter);
			Console.WriteLine($"Preprocessing {contract.Name}...");
			PreprocessContract(contract);
			// actually work with byte[] of each contract
			Console.WriteLine(Config.ConsoleDelimeter);
			Console.WriteLine($"Analyzing {contract.Name}...");

			//var result = new AnalyzedContractModel();

			foreach (ContractPage page in contract.ContractPages)
			{
				Mat src = Cv2.ImRead(page.Path, ImreadModes.Grayscale);
				var bin = new Mat();
				_ = Cv2.Threshold(src, bin, 0, 255, ThresholdTypes.BinaryInv);

				Cv2.FindContours(bin, out Point[][] contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
				var color = new Mat(src.Rows, src.Cols, MatType.CV_8UC3, new Scalar(0, 0, 0));
				for (int i = 0; i < contours.Length; i++)
				{
					Rect rect = Cv2.BoundingRect(contours[i]);
					if (rect.Width < 15 || rect.Height < 15)
						continue;
					Cv2.Rectangle(color, rect, Scalar.Red, 2);
				}
				if (src.Rows != color.Rows || src.Cols != color.Cols)
				{
					Cv2.Resize(color, src, new Size(src.Cols, src.Rows));
				}

				var result = new Mat();
				Cv2.CvtColor(src, result, ColorConversionCodes.GRAY2BGR);
				Cv2.AddWeighted(result, 0.5, color, 0.5, 0, result);
				Cv2.ImShow("Result", result);
				_ = Cv2.WaitKey();
			}
			return null;
		}

		private void PreprocessContract(Contract contract)
		{
			_ = contract.Extension switch
			{
				".pdf" => SaveAsImage(contract),
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

		private dynamic SaveAsImage(Contract contract)
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
				Density = new Density(Config.Density.x, Config.Density.y, DensityUnit.PixelsPerCentimeter)
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
				contract.ContractPages.Add(new() { Path = Path.Combine(pdfDir, $"page_{page}.png") });
				image.Write(Path.Combine(pdfDir, $"page_{page}.png"));
				page++;
			}

			return 0;
		}
	}
}
