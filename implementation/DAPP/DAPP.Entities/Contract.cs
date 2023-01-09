namespace DAPP.Entities
{
	public sealed class Contract
	{

		public Contract(int id, string filePath)
		{
			Id = id;
			FilePath = filePath;
			Name = Path.GetFileNameWithoutExtension(filePath);
			Extension = Path.GetExtension(filePath);
		}

		public int Id { get; set; }
		/// <summary>
		///  Name of file
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Full fillepath including Extension
		/// </summary>
		public string? FilePath { get; set; }

		/// <summary>
		///  Extension, e.g. .pdf
		/// </summary>
		public string Extension { get; set; }
	}
}
