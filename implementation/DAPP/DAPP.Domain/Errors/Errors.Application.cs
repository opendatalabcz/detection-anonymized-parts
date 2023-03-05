namespace DAPP.Domain.Errors
{
	using ErrorOr;

	public static partial class Errors
	{
		public static class Application
		{
			public static Error FileTypeNotSupported => Error.Failure(
				code: "Application.FileTypeNotSupported",
				description: "Filetype is not supported by the application. Consider using supported formats.");

			public static Error NotImplemented => Error.Failure(
				code: "Application.NotImplemented",
				description: "The functionality is not implemented yet.");

		}
	}
}
