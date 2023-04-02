namespace DAPP.Domain.Common
{
	public sealed class ScanType
	{
		public static ScanTypeEnum Scanned => ScanTypeEnum.Scanned;
		public static ScanTypeEnum Digital => ScanTypeEnum.Digital;
		public static ScanTypeEnum NotDetermined => ScanTypeEnum.NotDetermined;
	}

	public enum ScanTypeEnum
	{
		Scanned = 1,
		Digital = 2,
		NotDetermined = 3,
	}
}
