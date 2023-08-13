using Infrastructure.Services;

namespace Unit.Tests;

public class DappFileHandleServiceTests
{
    // Add multiple test cases for internet and local file
    [Theory]
    [InlineData("https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf")]
    [InlineData("../../../TestFiles/1.pdf")]
    public async Task LoadPdf(string path)
    {
        var fileHandleService = new FileHandleService();
        var result = await fileHandleService.GetBytes(path);
        Assert.True(!result.IsError);
        Assert.NotNull(result.Value);
        Assert.True(result.Value.Length > 0);
    }
}

public class DateTimeProviderServiceTests
{
    [Fact]
    public void DateTimeProviderService_ShouldReturnValidDateTime_WhenCalled()
    {
        // Arrange
        var dateTimeProviderService = new DateTimeProvider();

        // Act
        var result = dateTimeProviderService.UtcNow;

        // Assert
        Assert.NotNull(result);
        Assert.True(result > DateTime.MinValue);
        Assert.True(result < DateTime.MaxValue);
    }
}