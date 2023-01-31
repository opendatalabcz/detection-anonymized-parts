namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;

	public sealed class LoadSingleContractOperation : ILoadSingleContractOperation
	{
		private readonly IContractRepository contractRepository;
		public LoadSingleContractOperation(IContractRepository contractRepository)
		{
			this.contractRepository = contractRepository;
		}

		public int Execute(string contractPath)
		{
			var fileInfo = new FileInfo(contractPath);
			Console.WriteLine($"{fileInfo.Name}");
			return contractRepository.AddContract(fileInfo.FullName);
		}
	}
}