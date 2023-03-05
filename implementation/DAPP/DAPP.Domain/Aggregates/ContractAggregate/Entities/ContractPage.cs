namespace DAPP.Domain.Aggregates.ContractAggregate.Entities
{
	using System.Diagnostics;

	[DebuggerDisplay("Id: {Id}, HasAnonymizedParts: {HasAnonymizedParts}, IsGrayscale: {IsGrayscale}")]
	public sealed class ContractPage
	{
		public required int Id { get; set; }

		public required Contract Contract { get; set; }
	}
}
