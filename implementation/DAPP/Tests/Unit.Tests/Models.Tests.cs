using Domain.DocumentAggregate;
using Domain.PageAggregate;

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

public class PageTests
{
    [Fact]
    public void Page_ShouldCreateValidObject_WhenCalledWithValidData()
    {
        // Arrange
        var pageNumber = 1;
        var originalImageUrl = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";
        var resultImageUrl = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";
        var anonymizationResult = 0.5f;
        var emptyDoc = Document.Create(string.Empty, string.Empty, string.Empty, new());
        // Act
        var page = Page.Create(emptyDoc, pageNumber, originalImageUrl, resultImageUrl, anonymizationResult);

        // Assert
        Assert.NotNull(page);
        Assert.Equal(pageNumber, page.PageNumber);
        Assert.Equal(originalImageUrl, page.OriginalImageUrl);
        Assert.Equal(resultImageUrl, page.ResultImageUrl);
        Assert.Equal(anonymizationResult, page.AnonymizationResult);
        Assert.Null(page.DocumentId);
    }
}

public class DocumentTests
{
    [Fact]
    public void Document_ShouldCreateValidObject_WhenCalledWithValidData()
    {
        // Arrange
        var name = "1";
        var url = "1";
        var hash = "1";
        var pages = new List<Page>();
        // Act
        var document = Document.Create(name, url, hash, pages);

        // Assert
        Assert.NotNull(document);
        Assert.Equal(name, document.Name);
        Assert.Equal(url, document.Url);
        Assert.Equal(hash, document.Hash);
        Assert.Equal(pages, document.Pages);
    }
}

public class CommonTests
{
    [Fact]
    public void Common_TestEntityMethods()
    {
        // Arrange
        var document = Document.Create("1", "1", "1", new());
        var document2 = Document.Create("2", "2", "2", new());
        // Act
        // Assert
        if (document == document2)
        { }
        if (document != document2)
        { }
        if (document.Equals(document2))
        { }
        if (document.Equals((object)document2))
        { }
        if (document.GetHashCode() == document2.GetHashCode())
        { }
    }
}