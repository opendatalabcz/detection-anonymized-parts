namespace DAPP.Controllers
{
	using DAPP.BusinessLogic.Interfaces.Facades;

	public sealed class AnalyzerController
	{
		private readonly IAnalyzerFacade analyzerFacade;

		public AnalyzerController(IAnalyzerFacade analyzerFacade)
		{
			this.analyzerFacade = analyzerFacade;
		}

		public void LoadContracts(string contractsFolderPath)
		{
			analyzerFacade.LoadContracts(contractsFolderPath);
		}

		public void Run()
		{
			analyzerFacade.Run();
		}
	}
}