namespace DAPP.Analyzer.Errors
{
	using ErrorOr;
	public static class Analyzer
	{
		public static class Validation
		{
			public static class Image
			{
				public static Error IsNull =>
				Error.Validation(
					code: "Analyzer.Validation.Image.IsNull",
					description: "Provided image was null.");
				public static Error InvalidDimension =>
				Error.Validation(
					code: "Analyzer.Validation.Image.InvalidDimension",
					description: "Provided image has at least one dimension equal to zero.");
			}
		}
	}
}

