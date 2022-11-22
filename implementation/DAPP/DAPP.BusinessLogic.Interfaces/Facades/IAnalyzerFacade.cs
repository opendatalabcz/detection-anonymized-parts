namespace DAPP.BusinessLogic.Interfaces.Facades
{
	public interface IAnalyzerFacade
	{

		/// <summary>
		/// Loads contracts and saves them to repository as Contract entity
		/// </summary>
		/// <param name="contractsFolderPath">Path to folder on disk</param>
		void LoadContracts(string contractsFolderPath);
		/// <summary>
		/// Runs analysis of contracts
		/// <para>TODO: Add parameters</para>
		/// </summary>
		void Run();
	}
}
