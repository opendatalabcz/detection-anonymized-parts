namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;

	using ImageMagick;

	using System.Diagnostics;

	public sealed class LoadContractsOperation : ILoadContractsOperation
	{
		private readonly IContractRepository contractRepository;
		private (int, int) densitySettings;
		private string contractsFolder;
		public LoadContractsOperation(IContractRepository contractRepository, (int, int) densitySettings)
		{
			this.contractRepository = contractRepository;
			this.densitySettings = densitySettings;
		}

		public void Execute(string contractsFolderPath)
		{

			// for each file in contractsFolderPath call contractRepository.AddContract
			var directoryInfo = new DirectoryInfo(contractsFolderPath);
			contractsFolder = contractsFolderPath
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				Console.WriteLine($"{fileInfo.Name}");
				contractRepository.AddContract(fileInfo.FullName);
			}

			foreach (Contract contract in contractRepository.GetAllContracts())
			{
				Console.WriteLine($"Analyzing {contract.Name}...");
				_ = contract.Extension switch
				{
					".pdf" => SaveAsImage(contract),
					".jpg" => CopyToImagesFolder(contract),
					".jpeg" => CopyToImagesFolder(contract),
					".png" => CopyToImagesFolder(contract),
					_ => throw new UnreachableException(),
				};
			}

		}


		private dynamic CopyToImagesFolder(Contract contract)
		{
			File.Copy(contract.FilePath, contractsFolder + )
				return 0;
		}

		private dynamic SaveAsImage(Contract contract)
		{
			// Load pdf // Get list of individual pages as Image object
			// For each page change color of all text to white;
			string pdfFileName = Path.Combine("pngs", contract.Name);
			string pngFolderPath = Path.GetDirectoryName(contract.FilePath) + "\\.." + "\\pngs";
			if (!Directory.Exists(pngFolderPath))
			{
				Directory.CreateDirectory(pngFolderPath);
			}

			var settings = new MagickReadSettings
			{
				Density = new Density(densitySettings.Item1, densitySettings.Item2, DensityUnit.PixelsPerCentimeter)
			};

			using var images = new MagickImageCollection();
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

			return 0;
		}

	}
}
