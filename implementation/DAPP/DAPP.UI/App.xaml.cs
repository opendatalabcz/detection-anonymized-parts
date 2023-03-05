
namespace DAPP.UI
{
	using System.Windows;

	using DAPP.API;
	using DAPP.DI;

	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public DAPPController controller;
		private void OnStartup(object sender, StartupEventArgs e)
		{
			var servicecoll = CfgWPFServices();
			var services = DependencyInjectionService.ConfigureServices(servicecoll);

			using var scope = services.CreateScope();
			this.controller = scope.ServiceProvider.GetService<DAPPController>();
		}

		private ServiceCollection CfgWPFServices()
		{
			var services = new ServiceCollection();
			services.AddScoped<DAPPController>();
			return services;
		}
	}
}
