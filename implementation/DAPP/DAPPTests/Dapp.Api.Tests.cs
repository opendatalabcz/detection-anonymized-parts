namespace DAPPTests;

public class DappFileHandleServiceTests
{
    // Add multiple test cases for internet and local file
    [Theory]
    [InlineData("https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf")]
    [InlineData("../../../TestFiles/1.pdf")]
    public async Task LoadPdf(string path)
    {
        var result = await FileHandleService.GetBytes(path);
        Assert.True(!result.IsError);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.Length > 0);
    }
}

public class RequestHandlerTests
{

}

