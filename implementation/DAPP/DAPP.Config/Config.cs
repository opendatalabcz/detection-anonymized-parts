public static class Config
{
	public static string ContractsFolderPath { get; } = @"../../../../TestData/pdfs/";
	public static string ResultsFolderPath { get; } = @"../../../../Results/";

	public static string ConsoleDelimeter { get; } = "|--------------------------------------------------------------|";

	public static (int x, int y) Density { get; } = (100, 100);

	public static bool LoadAllThenAnalyzeAll = true;
}
