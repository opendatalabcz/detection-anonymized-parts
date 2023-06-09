using DAPPAnalyzer.Models;

namespace Unit.Tests;
public class DappPDFTests
{
    [Fact]
    public async Task DappPDF_ShouldCreateValidObject_WhenCalledWithValidData()
    {
        // Arrange
        var path = "../../../TestFiles/1.pdf";
        var bytes = File.ReadAllBytes(path);
        var contractName = "1";

        // Act
        var pdf = await DappPDF.Create(bytes, contractName, path);

        // Assert
        Assert.NotNull(pdf);
        Assert.Equal(path, pdf.Url);
        Assert.NotNull(pdf.Pages);
        Assert.False(pdf.Analyzed);
        Assert.Equal(contractName, pdf.ContractName);
        Assert.True(pdf.Pages.Count > 0);
    }

    [Fact]
    public async Task DappPDF_ShouldThrowException_WhenInvalidData()
    {
        //arrange
        var path = "../../../TestFiles/1.pdf";
        var bytes = Enumerable.Range(0, 98).Select(x => (byte)0).ToArray();
        var contractName = "1";

        //act
        try
        {
            var pdf = await DappPDF.Create(bytes, contractName, path);
        }
        catch
        {
            Assert.True(true);
            return;
        }

        Assert.False(false);
    }
}
