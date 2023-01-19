namespace DAPP.BusinessLogic.Operations
{
	using DAPP.BusinessLogic.Interfaces.Operations;
	using DAPP.BusinessLogic.Interfaces.Repositories;
	using DAPP.Entities;
	using DAPP.Models;
	using System.Diagnostics;
	using ImageMagick;

	public sealed class AnalyzeSingleContractOperation : IAnalyzeSingleContractOperation
	{
		private readonly IContractRepository contractRepository;
		
		public AnalyzeSingleContractOperation(IContractRepository contractRepository)
		{
			this.contractRepository = contractRepository;
		}

		public AnalyzedContractModel Execute(Contract contract)
		{
			throw new NotImplementedException();
		}
	}
}