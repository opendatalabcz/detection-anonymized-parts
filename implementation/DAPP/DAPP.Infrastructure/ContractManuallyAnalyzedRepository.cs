namespace DAPP.Infrastructure
{
	using DAPP.Application.Interfaces;
	using DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate;
	using DAPP.Domain.Aggregates.ManuallyAnalyzedResultAggregate.Entities;
	using DAPP.Domain.Common;

	public sealed class ContractManuallyAnalyzedRepository : IContractManualAnalyzeRepository
	{
		// csv file in format 
		// contractName | scantypeEnum | id:hasAnonymizedPart, id:hasAnonymizedPart,...

		private static List<ManuallyAnalyzedResult> manuallyAnalyzedResults = new();
		public ContractManuallyAnalyzedRepository()
		{
			InitializeList();

		}

		public int AddManuallyAnalyzedContract(ManuallyAnalyzedResult result)
		{
			result.Id = manuallyAnalyzedResults.Count;
			manuallyAnalyzedResults.Add(result);
			return result.Id;
		}

		private void InitializeList()
		{
			string csv = @"..\..\..\..\manually_analyzed.csv";
			if (!File.Exists(csv))
			{
				File.Create(csv);
			}

			foreach (var row in File.ReadLines(csv))
			{
				var splitrow = row.Split('|');
				var contractName = splitrow[0];
				ScanTypeEnum stEnum = (ScanTypeEnum)(int.Parse(splitrow[1]));
				List<(int, int)> hasAnonymizedParts = splitrow[2].Split(',')
					.Select(x =>
					(int.Parse(x.Split(':')[0]),
					int.Parse(x.Split(':')[1])))
					.ToList();

				var mar = new ManuallyAnalyzedResult()
				{
					ContractName = contractName,
					TypeOfScan = stEnum,
					ManuallyAnalyzedPages = new()
				};
				foreach (var item in hasAnonymizedParts)
				{
					mar.ManuallyAnalyzedPages.Add(new ManuallyAnalyzedPage { HasAnonymizedPart = item.Item2 == 0, Id = item.Item1 });
				};
				manuallyAnalyzedResults.Add(mar);
			}
		}
	}
}
