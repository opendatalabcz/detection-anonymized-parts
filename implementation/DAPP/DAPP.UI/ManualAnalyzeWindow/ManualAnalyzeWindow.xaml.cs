namespace DAPP.UI.ManualAnalyzeWindow
{
	using System.Windows;

	using DAPP.API;

	/// <summary>
	/// Interaction logic for ManualAnalyzeWindow.xaml
	/// </summary>
	public partial class ManualAnalyzeWindow : Window
	{
		private readonly DAPPController controller;
		public ManualAnalyzeWindow(DAPPController controller)
		{
			InitializeComponent();
			this.controller = controller;
		}
	}
}
