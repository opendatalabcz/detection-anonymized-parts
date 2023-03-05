
namespace DAPP.UI
{
	using System.Windows;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ManualAnalyzeWindow.ManualAnalyzeWindow manual;
		public MainWindow(ManualAnalyzeWindow.ManualAnalyzeWindow manual)
		{
			InitializeComponent();
			this.manual = manual;
		}

		private void ManualModeButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AutomaticModeButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
