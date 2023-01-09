namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;
	using DAPP.Models;
	using System.Diagnostics;
	using ImageMagick;

	public sealed class AnalyzeSingleContractOperation : IAnalyzeSingleContractOperation
	{
		private readonly IContractRepository contractRepository;
		private (int, int) densitySettings;
		public AnalyzeSingleContractOperation(IContractRepository contractRepository, (int, int) densitySettings)
		{
			this.contractRepository = contractRepository;
			this.densitySettings = densitySettings;
		}

		public AnalyzedContractModel Execute(Contract contract)
		{
			Console.WriteLine($"Analyzing {contract.Name}...");
			return contract.Extension switch
			{
				".pdf" => AnalyzePdf(contract),
				".jpg" => throw new NotImplementedException(),
				".jpeg" => throw new NotImplementedException(),
				".png" => throw new NotImplementedException(),
				_ => throw new UnreachableException(),
			};
		}

		private AnalyzedContractModel AnalyzePdf(Contract contract)
		{
			// Load pdf // Get list of individual pages as Image object
			// For each page change color of all text to white;
			string pdfFileName = Path.Combine("pngs", contract.Name);
			string pngFolderPath = Path.GetDirectoryName(contract.FilePath) + "\\.." + "\\pngs";
			if (!Directory.Exists(pngFolderPath))
			{
				Directory.CreateDirectory(pngFolderPath);
			}

			MagickReadSettings settings = new MagickReadSettings();
			settings.Density = new Density(densitySettings.Item1, densitySettings.Item2, DensityUnit.PixelsPerCentimeter);

			using (MagickImageCollection images = new MagickImageCollection())
			{
				images.Read(contract.FilePath);

				string pdfDir = Path.Combine(pngFolderPath, contract.Name);
				if (!Directory.Exists(pdfDir))
				{
					Directory.CreateDirectory(pdfDir);
				}

				Console.WriteLine($"Saving {contract.Name} to pngs...");
				Console.WriteLine($"Total pages:{images.Count()}");
				// iterate over all images
				int page = 1;
				foreach (MagickImage image in images)
				{
					Console.WriteLine($"Saving page number {page} to: page_{page}.png ...");
					// save 
					image.Write(Path.Combine(pdfDir, $"page_{page}.png"));
					page++;
				}

				return null;
			}
		}

		//private void SavePdfAsIndividualPagesAsPng(Contract contract)
		//{
		//	string dest = contract.Id + "\\" + contract.Name + "_removed_outlines.pdf";
		//	var file = new FileInfo(dest);
		//	file.Directory.Create();
		//	var reader = new PdfReader(contract.FilePath);
		//	var writer = new PdfWriter(dest);
		//	var pdfDocument = new PdfDocument(reader, writer);
		//	int pages = pdfDocument.GetNumberOfPages();
		//	for (int i = 1; i <= pages; i++)
		//	{
		//		_ = pdfDocument.GetPage(i);
		//		throw new NotImplementedException();
		//	}
		//	pdfDocument.Close();
		//	pdfDocument.Close();
		//}
	}
}