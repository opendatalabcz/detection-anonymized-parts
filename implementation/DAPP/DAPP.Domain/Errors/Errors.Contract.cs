namespace DAPP.Domain.Errors
{
	using ErrorOr;

	public static partial class Errors
	{
		public static class Contract
		{
			public static Error NoPagesFound => Error.NotFound(
				code: "Contract.NoPagesFound",
				description: "Contract contains no pages.");
		}
	}
}
