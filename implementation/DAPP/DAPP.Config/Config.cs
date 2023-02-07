public static class Config
{
	public static string ContractsFolderPath { get; } = @"../../../../TestData/pdfs/";
	public static string ResultsFolderPath { get; } = @"../../../../Results/";

	public static string ConsoleDelimeter { get; } = "|--------------------------------------------------------------|";

	public static (int x, int y) Density { get; } = (75, 75);
	public static (int x, int y) KSize { get; } = (101, 101);

	public static bool LoadAllThenAnalyzeAll = true;

	public static int SegmentsCount { get; } = 50000;
	public static string TestDataFolderPath { get; set; } = @"../../../../TestData/";
}
