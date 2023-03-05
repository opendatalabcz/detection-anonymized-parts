namespace DAPP.Domain.Errors
{
	using ErrorOr;

	public static partial class Errors
	{
		public static class IO
		{
			public static Error FileNotFound => Error.NotFound(
				code: "IO.FileNotFound",
				description: "File not found.");
		}
	}
}

