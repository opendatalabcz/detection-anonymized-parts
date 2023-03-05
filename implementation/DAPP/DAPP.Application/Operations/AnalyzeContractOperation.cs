namespace DAPP.Application.Operations
{
	using DAPP.Application.Interfaces;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;
	using DAPP.Domain.Aggregates.ContractAggregate;
	using DAPP.Domain.Common;

	using ErrorOr;

	using ImageMagick;

	public sealed class AnalyzeContractOperation
	{
		private readonly IAnalyzer analyzer;

		public AnalyzeContractOperation(IAnalyzer analyzer)
		{
			this.analyzer = analyzer;
		}

		public ErrorOr<AnalyzedContract> Execute(Contract contract)
		{
			Console.WriteLine($"Analyzing {contract.Extension}: {contract.Name}...");

			var result = new AnalyzedContract()
			{
				Contract = contract,
				AnalyzedPages = new(),
			};

			// pdf containing (most likely) multiple pages
			if (contract.Extension == FileExtensionEnum.Pdf)
			{
				int i = 0;
				var pages = new MagickImageCollection(contract.Path);
				foreach (MagickImage page in pages)
				{
					page.Quality = 100;
					var analyzed = analyzer.AnalyzePage(page, contract.Pages[i++]);
					result.AnalyzedPages.Add(analyzed);
				}
			}

			// just one image
			else
			{
				var page = new MagickImage(contract.Path);

				var analyzed = analyzer.AnalyzePage(page, contract.Pages[0]);
				analyzed.Value.ContractPage = contract.Pages[0];
				result.AnalyzedPages.Add(analyzed);
			}
			return result;
		}
	}
}