namespace DAPP.Domain.Common
{
	public static class FileExtension
	{
		public static bool TryGetExtension(string v, out FileExtensionEnum? e)
		{
			if (string.IsNullOrEmpty(v))
			{
				e = null;
				return false;
			}
			if (v == "jpg" || v == "Jpg" || v == "JPG")
			{
				e = FileExtensionEnum.Jpg;
				return true;
			}

			if (v == "jpeg" || v == "Jpeg" || v == "JPEG")
			{
				e = FileExtensionEnum.Jpeg;
				return true;
			}

			if (v == "png" || v == "Png" || v == "PNG")
			{
				e = FileExtensionEnum.Png;
				return true;
			}
			if (v == "pdf" || v == "Pdf" || v == "PDF")
			{
				e = FileExtensionEnum.Pdf;
				return true;
			}

			e = null;
			return false;
		}

		public static FileExtensionEnum Jpg => FileExtensionEnum.Jpg;
		public static FileExtensionEnum Jpeg => FileExtensionEnum.Jpeg;
		public static FileExtensionEnum Png => FileExtensionEnum.Png;
		public static FileExtensionEnum Pdf => FileExtensionEnum.Pdf;
	}
	public enum FileExtensionEnum
	{
		Jpg = 1,
		Jpeg = 2,
		Png = 3,
		Pdf = 4,
	}
}
