namespace DAPP.Repositories
{
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;
	using DAPP.Models;

	using System.Collections.Generic;

	public sealed class ContractRepository : IContractRepository
	{
		private readonly List<Contract> Contracts = new();
		private readonly List<AnalyzedContractModel> AnalyzedContractModels = new();

		public void AddContract(string s)
		{
			Contracts.Add(new Contract(Contracts.Count, s));
		}

		public IEnumerable<Contract> GetAllContracts()
		{
			foreach (Contract c in Contracts)
			{
				yield return c;
			}
		}

		public Contract? GetContract(int contractId)
		{
			return Contracts.Find(c => c.Id == contractId);
		}

		public void SaveAnalyzedContract(AnalyzedContractModel analyzedContractModel)
		{
			AnalyzedContractModels.Add(analyzedContractModel);

		}
	}
}