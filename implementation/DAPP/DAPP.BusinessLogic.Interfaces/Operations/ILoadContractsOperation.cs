namespace DAPP.BusinessLogic.Interfaces.Operations
{
	public interface ILoadContractsOperation
	{
		/// <summary>
		/// Loads Contracts and saves them to repository
		/// </summary>
		public void Execute(string contractsFolderPath);
	}
}
