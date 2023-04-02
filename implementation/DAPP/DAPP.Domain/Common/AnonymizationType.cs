namespace DAPP.Domain.Common
{
	public sealed class AnonymizationType
	{
		public static AnonymizationTypeEnum None => AnonymizationTypeEnum.None;
		public static AnonymizationTypeEnum BlackRectangle => AnonymizationTypeEnum.BlackRectangle;
		public static AnonymizationTypeEnum RandomNoise => AnonymizationTypeEnum.RandomNoise;
		public static AnonymizationTypeEnum ColoredSticker => AnonymizationTypeEnum.ColoredSticker;
		public static AnonymizationTypeEnum Other => AnonymizationTypeEnum.Other;
	}

	public enum AnonymizationTypeEnum
	{
		None = 1,
		BlackRectangle = 2,
		RandomNoise = 3,
		ColoredSticker = 4,
		Other = 5,
	}
}
