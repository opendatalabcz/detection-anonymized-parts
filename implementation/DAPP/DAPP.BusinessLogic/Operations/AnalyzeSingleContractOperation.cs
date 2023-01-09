namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;
	using DAPP.Models;
	using iText.Kernel.Colors;
	using iText.Kernel.Font;
	using iText.Kernel.Pdf;
	using iText.Kernel.Pdf.Annot;
	using iText.Kernel.Pdf.Canvas;
	using iText.Kernel.Pdf.Canvas.Parser;
	using iText.Kernel.Pdf.Canvas.Parser.Listener;
	using System.Diagnostics;
	using System.Text;
	using System.Text.RegularExpressions;

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
			// Load pdf // Get list of individual pages
			// For each page change color of all text to white;
			var outputPath = contract.FilePath.Replace(contract.Extension, "") + "\\whitened_text" + contract.Extension;
			Directory.CreateDirectory(contract.FilePath.Replace(contract.Extension, ""));
			using (PdfDocument pdf = new PdfDocument(new PdfReader(contract.FilePath), new PdfWriter(outputPath)))
			{
				//TODO : trigger for prints
				Console.WriteLine("Whitening text in pages for furhter processing...");
				for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
				{
					//TODO : trigger for prints
					Console.WriteLine($"Page {i}...");
					PdfPage page = pdf.GetPage(i);
					var strategy = new LocationTextExtractionStrategy();
					PdfCanvasProcessor processor = new PdfCanvasProcessor(strategy);
					processor.ProcessPageContent(page);
					var text = strategy.GetResultantText();
					Console.WriteLine(text);
					// text = Regex.Replace(text, "1 g", "1 g"); // change color to white
					byte[] data = Encoding.UTF8.GetBytes(text);
					PdfStream stream = new PdfStream(data);
					stream.Put(PdfName.Filter, PdfName.FlateDecode);
					page.Put(PdfName.Contents, stream);
				}
				pdf.Close();
				//TODO : trigger for prints
				Console.WriteLine("Whitening finished\n");
			}
			return null;
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