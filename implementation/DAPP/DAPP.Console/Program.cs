#define TEST
namespace DAPP.Console
{

	using DAPP.API;
	using DAPP.DI;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate.Entities;
	using DAPP.Domain.Aggregates.ContractAggregate;

	using ErrorOr;

	using Microsoft.Extensions.DependencyInjection;

	internal class Program
	{
		private static void Main(string[] args)
		{
			var services = DependencyInjectionService.ConfigureServices(new ServiceCollection());

			using var scope = services.CreateScope();
			var controller = scope.ServiceProvider.GetService<DAPPController>();

#if TEST
			// analyze test data
			foreach (var result in AnalyzeTestData(controller))
			{
				if (!result.IsError)
					controller.AddAnalyzedContract(result.Value);
			}

			// print to console
			foreach (var analyzedContract in controller.GetAnalyzedContracts())
			{
				(Contract c, List<ErrorOr<AnalyzedPage>> pages) = analyzedContract;

				System.Console.WriteLine($"Contract: {c.Name}");
				System.Console.WriteLine($"Pages: {pages.Count}");
				foreach (var page in pages)
				{
					System.Console.Write('\t');
					if (page.IsError)
					{
						System.Console.WriteLine($"Error: {page.FirstError}");
					}
					else
					{
						System.Console.WriteLine($"Page: {page.Value.ContractPage.Id + 1}");
						System.Console.Write('\t');
						System.Console.WriteLine($"Anonymization type: {page.Value.AnonymizationType}");
					}
				}
			}
		}
#endif
#if TEST
		private static IEnumerable<ErrorOr<AnalyzedContract>> AnalyzeTestData(DAPPController controller)
		{
			DirectoryInfo pdfsDirectoryInfo = new DirectoryInfo(@"..\..\..\..\TestData\pdfs\");
			foreach (var file in pdfsDirectoryInfo.GetFiles())
			{
				yield return controller.AnalyzeContract(file.FullName);
			}

			DirectoryInfo samplesDirectoryInfo = new DirectoryInfo(@"..\..\..\..\TestData\samples\");
			foreach (var file in samplesDirectoryInfo.GetFiles())
			{
				yield return controller.AnalyzeContract(file.FullName);
			}

		}
#endif

	}
}