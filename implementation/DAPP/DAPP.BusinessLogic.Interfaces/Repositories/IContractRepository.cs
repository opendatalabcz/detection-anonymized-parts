namespace DAPP.BusinessLogic.Interfaces.Repositories
{
	using DAPP.Entities;
	using DAPP.Models;

	using System.Collections.Generic;

	public interface IContractRepository
	{
		/// <summary>
		/// Adds a contract to the repository
		/// </summary>
		/// <param name="s"></param>
		public void AddContract(string s);

		/// <summary>
		/// Gets all contracts
		/// </summary>
		/// <returns>IEnumerabe of all contracts</returns>
		IEnumerable<Contract> GetAllContracts();

		/// <summary>
		/// Returns a contract with the specified id
		/// </summary>
		/// <param name="contractId"> Id of the contract to return </param>
		/// <returns></returns>
		Contract? GetContract(int contractId);

		/// <summary>
		/// Saves an analyzed contract to the repository
		/// </summary>
		/// <param name="analyzedContractModel"></param>
		void SaveAnalyzedContract(AnalyzedContractModel analyzedContractModel);
	}
}
