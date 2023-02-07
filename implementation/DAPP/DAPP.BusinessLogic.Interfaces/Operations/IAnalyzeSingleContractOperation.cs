namespace DAPP.BusinessLogic.Interfaces.Operations
{
	using DAPP.Models;

	public interface IAnalyzeSingleContractOperation
	{
		/// <summary>
		/// Runs analysis of a contract
		/// </summary>
		public List<AnalyzedContractModel> Execute(int contractId);
	}
}
