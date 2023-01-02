namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;
	using DAPP.Models;

	using iText.Kernel.Pdf;

	using System.Diagnostics;
	public sealed class AnalyzeSingleContractOperation : IAnalyzeSingleContractOperation
	{
		private readonly IContractRepository contractRepository;

		public AnalyzeSingleContractOperation(IContractRepository contractRepository)
		{
			this.contractRepository = contractRepository;
		}

		public AnalyzedContractModel Execute(Contract contract)
		{
			Console.WriteLine($"Analyzing {contract.Name}...");
			return contract.ContractFileType switch
			{
				ContractFileType.pdf => AnalyzePdf(contract),
				ContractFileType.jpg => throw new NotImplementedException(),
				ContractFileType.jpeg => throw new NotImplementedException(),
				ContractFileType.png => throw new NotImplementedException(),
				_ => throw new UnreachableException(),
			};
		}

		private AnalyzedContractModel AnalyzePdf(Contract contract)
		{
			SavePdfAsIndividualPagesAsPng(contract);
			throw new NotImplementedException();
		}

		private void SavePdfAsIndividualPagesAsPng(Contract contract)
		{
			string dest = contract.Id + "\\" + contract.Name + "_removed_outlines.pdf";
			var file = new FileInfo(dest);
			file.Directory.Create();
			var reader = new PdfReader(contract.FilePath);
			var writer = new PdfWriter(dest);
			var pdfDocument = new PdfDocument(reader, writer);
			int pages = pdfDocument.GetNumberOfPages();
			for (int i = 1; i <= pages; i++)
			{
				_ = pdfDocument.GetPage(i);
				throw new NotImplementedException();
			}
			pdfDocument.Close();
			pdfDocument.Close();
		}
	}
}