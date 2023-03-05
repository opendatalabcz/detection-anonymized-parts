namespace DAPP.Application.Interfaces
{
	using System.Collections.Generic;

	using DAPP.Domain.Aggregates.AnalyzedResultAggregate;

	public interface IAnalyzedContractRepository
	{
		int AddAnalyzedContractResult(AnalyzedContract result);
		IEnumerable<AnalyzedContract> GetAnalyzedContracts();
	}
}
