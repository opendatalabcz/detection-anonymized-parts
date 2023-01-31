namespace DAPP.BusinessLogic.Interfaces.Operations
{
	public interface ILoadSingleContractOperation
	{
		/// <summary>
		/// Loads a single contract and saves it to repository
		/// <para>Returns id of saved contract</para>
		/// </summary>
		public int Execute(string contractPath);
	}
}
