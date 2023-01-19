namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;

	public sealed class LoadContractsOperation : ILoadContractsOperation
	{
		private readonly IContractRepository contractRepository;
		private (int, int) densitySettings;
		private readonly string contractsFolder;
		public LoadContractsOperation(IContractRepository contractRepository, (int, int) densitySettings)
		{
			this.contractRepository = contractRepository;
			this.densitySettings = densitySettings;
		}

		public void Execute(string contractsFolderPath)
		{

			// for each file in contractsFolderPath call contractRepository.AddContract
			var directoryInfo = new DirectoryInfo(contractsFolderPath);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				Console.WriteLine($"{fileInfo.Name}");
				contractRepository.AddContract(fileInfo.FullName);
			}

		}
	}
}
