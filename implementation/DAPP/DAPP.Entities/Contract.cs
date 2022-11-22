namespace DAPP.Entities
{
	public sealed class Contract
	{

		public Contract(int id, string filePath)
		{
			Id = id;
			FilePath = filePath;
			Name = Path.GetFileNameWithoutExtension(filePath);
		}

		public int Id { get; set; }
		public string? Name { get; set; }
		public string? FilePath { get; set; }
	}
}
