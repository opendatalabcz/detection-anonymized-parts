namespace DAPP.BusinessLogic.Interfaces.Operations
{
	using DAPP.Entities;
	using DAPP.Models;

	public interface IGetBlackBoundingBoxesOperation
	{
		string Name { get; }
		List<BoundingBoxModel> Execute(ContractPage page);
	}
}
