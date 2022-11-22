namespace DAPP.BusinessLogic.Interfaces.Operations
{
	public interface IAnalyzeSingleContractOperation
	{
		/// <summary>
		/// Runs analysis of a contract
		/// </summary>
		public void Execute(int contractId);
	}
}
