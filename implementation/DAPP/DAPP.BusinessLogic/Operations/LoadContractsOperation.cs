namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;

	public sealed class LoadContractsOperation : ILoadContractsOperation
	{
		private readonly IContractRepository contractRepository;

		public LoadContractsOperation(IContractRepository contractRepository)
		{
			this.contractRepository = contractRepository;
		}

		public void Execute(string contractsFolderPath)
		{
			// for each file in contractsFolderPath call contractRepository.AddContract
			var directoryInfo = new DirectoryInfo(contractsFolderPath);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles().Where(x => Enum.TryParse<ContractFileType>(x.Extension[1..], out _)))
			{
				Console.WriteLine($"{fileInfo.Name}");
				contractRepository.AddContract(fileInfo.FullName);
			}

		}
	}
}
