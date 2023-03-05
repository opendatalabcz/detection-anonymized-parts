namespace DAPP.Domain.Common
{
	public sealed class ScanType
	{
		public static ScanTypeEnum Paperscan => ScanTypeEnum.Paperscan;
		public static ScanTypeEnum Digital => ScanTypeEnum.Digital;
		public static ScanTypeEnum NotDetermined => ScanTypeEnum.NotDetermined;
	}

	public enum ScanTypeEnum
	{
		Paperscan = 1,
		Digital = 2,
		NotDetermined = 3,
	}
}
