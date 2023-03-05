namespace DAPP.DI
{
	using DAPP.Analyzer;
	using DAPP.API;
	using DAPP.Application.Facades;
	using DAPP.Application.Interfaces;
	using DAPP.Application.Operations;
	using DAPP.Infrastructure;

	using Microsoft.Extensions.DependencyInjection;

	public static class DependencyInjectionService
	{
		public static ServiceProvider ConfigureServices(this ServiceCollection services)
		{
			// controllers
			services.AddScoped<DAPPController>();

			// facades
			services.AddScoped<ContractFacade>();

			// operations
			// contracts
			services.AddScoped<AnalyzeContractOperation>();
			services.AddScoped<CreateContractOperation>();
			services.AddScoped<AddAnalyzedContractOperation>();
			services.AddScoped<AddManuallyAnalyzedContractOperation>();
			services.AddScoped<GetAnalyzedContractsOperation>();
			// repositories
			services.AddScoped<IAnalyzedContractRepository, AnalyzedContractRepository>();

			// analyzer
			services.AddScoped<IAnalyzer, Analyzer>();

			return services.BuildServiceProvider();
		}
	}
}