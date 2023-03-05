namespace DAPP.Application.Interfaces
{
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities;
	using DAPP.Domain.Aggregates.ContractAggregate.Entities;

	using ErrorOr;

	using ImageMagick;

	public interface IAnalyzer
	{
		/// <summary>
		/// Analyzes an image of a document page.
		/// </summary>
		/// <param name="image">An image of a document page represented in ImageMagick library object MagickImage</param>
		/// <returns>Returns errors list if there was any problem regarding analyzing, or returns AnalyzedPage object</returns>
		public ErrorOr<AnalyzedPage> AnalyzePage(MagickImage image, ContractPage contractPage);
	}
}
