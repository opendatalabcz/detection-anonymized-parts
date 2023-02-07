namespace DAPP.Models
{
	public class BoundingBoxModel
	{
		public BoundingBoxModel(int X, int Y, int Width, int Height)
		{
			this.X = X;
			this.Y = Y;
			this.Width = Width;
			this.Height = Height;
		}
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
	}
}
