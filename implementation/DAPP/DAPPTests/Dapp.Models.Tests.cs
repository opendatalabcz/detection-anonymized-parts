using DAPPAnalyzer.Models;

namespace DAPPTests
{
    public class DappPDFTests
    {
        [Fact]
        public async Task CreateTest()
        {
            var pdf = await DappPDF.Create(File.ReadAllBytes("../../../TestFiles/1.pdf"));
            Assert.NotNull(pdf);
            Assert.NotNull(pdf.Pages);
            Assert.Single(pdf.Pages);
        }
    }
}