namespace DAPP.Infrastructure
{
	using System.Collections.Generic;

	using DAPP.Application.Interfaces;
	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;

	public sealed class AnalyzedContractRepository : IAnalyzedContractRepository
	{
		List<AnalyzedContract> results = new();
		public int AddAnalyzedContractResult(AnalyzedContract result)
		{
			result.Id = results.Count;
			results.Add(result);
			return result.Id;
		}

		public IEnumerable<AnalyzedContract> GetAnalyzedContracts()
		{
			return results;
		}
	}
}
