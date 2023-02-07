namespace DAPP.BusinessLogic.Interfaces.Facades
{
	using DAPP.Models;

	public interface IAnalyzerFacade
	{

		/// <summary>
		/// Loads contracts and saves them to repository as Contract entity
		/// </summary>
		/// <param name="contractsFolderPath">Path to folder on disk</param>
		void LoadContracts(string contractsFolderPath);
		/// <summary>
		/// Runs analysis of contracts
		/// <para>contractId - default -1, analyzes all contracts in repository</para>
		/// <para>if contractId is set to nonnegative value, analyzes selected contract with that id.</para>
		/// </summary>
		List<List<AnalyzedContractModel>> Run(int contractId);

		int LoadContract(string contractPath);
	}
}
