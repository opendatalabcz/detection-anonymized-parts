namespace DAPP.BusinessLogic.Interfaces.Operations
{
	using DAPP.Models;

	public interface IAnalyzeContractsOperation
	{
		/// <summary>
		/// Runs analysis for all contracts
		/// </summary>
		public List<List<AnalyzedContractModel>> Execute();
	}
}
