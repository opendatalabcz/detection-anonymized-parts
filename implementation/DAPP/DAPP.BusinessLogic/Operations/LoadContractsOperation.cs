namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;

	public sealed class LoadContractsOperation : ILoadContractsOperation
	{
		private readonly ILoadSingleContractOperation loadSingleContractOperation;
		public LoadContractsOperation(ILoadSingleContractOperation loadSingleContractOperation)
		{
			this.loadSingleContractOperation = loadSingleContractOperation;
		}

		public void Execute(string contractsFolderPath)
		{

			// for each file in contractsFolderPath call contractRepository.AddContract
			var directoryInfo = new DirectoryInfo(contractsFolderPath);
			foreach (FileInfo contractPath in directoryInfo.GetFiles())
			{
				_ = loadSingleContractOperation.Execute(contractPath.FullName);
			}
		}
	}
}
