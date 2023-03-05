namespace DAPP.Application.Operations
{
	using DAPP.Domain.Aggregates.ContractAggregate;
	using DAPP.Domain.Aggregates.ContractAggregate.Entities;
	using DAPP.Domain.Common;
	using DAPP.Domain.Errors;

	using ErrorOr;

	using iText.Kernel.Pdf;

	public sealed class CreateContractOperation
	{
		public ErrorOr<Contract> Execute(string filepath)
		{
			// check if file exists
			if (!File.Exists(filepath))
			{
				return Errors.IO.FileNotFound;
			}

			var f = new FileInfo(filepath);

			// determine whether its .pdf or an image or other 
			if (!FileExtension.TryGetExtension(filepath.Split('.')[^1], out FileExtensionEnum? ext))
			{
				return Errors.Application.FileTypeNotSupported;
			}

			var c = new Contract()
			{
				Name = filepath.Split("\\")[^1].Split('.')[0],
				Extension = (FileExtensionEnum)ext,
				Path = filepath
			};

			var pages = new List<ContractPage>() { };

			if (ext == FileExtensionEnum.Pdf)
			{
				// if pdf, open the pdf and get its properties
				using (PdfDocument pdf = new(new PdfReader(filepath)))
				{
					int pagesCount = pdf.GetNumberOfPages();
					for (int i = 1; i <= pagesCount; i++)
					{
						pages.Add(
							   new ContractPage()
							   {
								   Id = pages.Count,
								   Contract = c
							   });
					}
				}
			}
			// file is an image
			else
			{
				pages.Add(
					   new ContractPage
					   {
						   Id = pages.Count,
						   Contract = c
					   });
			}

			c.Pages = pages;

			return c;
		}
	}
}
