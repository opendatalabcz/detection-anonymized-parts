namespace DAPP.Entities
{
	public sealed class Contract
	{

		public Contract(int id, string filePath)
		{
			Id = id;
			FilePath = filePath;
			Name = Path.GetFileNameWithoutExtension(filePath);
			ContractFileType = Enum.Parse<ContractFileType>(Path.GetExtension(filePath)[1..].ToLower());
		}

		public int Id { get; set; }
		public string? Name { get; set; }
		public string? FilePath { get; set; }

		public ContractFileType ContractFileType { get; set; }
	}

	public enum ContractFileType
	{
		pdf,
		jpg,
		jpeg,
		png
	}
}
