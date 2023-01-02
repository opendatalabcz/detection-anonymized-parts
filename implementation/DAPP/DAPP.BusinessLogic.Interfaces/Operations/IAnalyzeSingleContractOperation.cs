namespace DAPP.BusinessLogic.Interfaces.Operations
{
	using DAPP.Entities;
	using DAPP.Models;

	public interface IAnalyzeSingleContractOperation
	{
		/// <summary>
		/// Runs analysis of a contract
		/// </summary>
		public AnalyzedContractModel Execute(Contract contract);
	}
}
