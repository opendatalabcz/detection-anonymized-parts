using Newtonsoft.Json.Linq;

namespace DAPP.Integration.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<API2.Program>>
{
    private readonly WebApplicationFactory<API2.Program> _factory;

    public ApiTests(WebApplicationFactory<API2.Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AnalyzeRequestHandler_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");
        request.Content = new StringContent("invalid request", Encoding.UTF8, "application/json");

        // Act
        var response = await client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AnalyzeRequestHandler_ShouldReturnError_WhenFileDoesNotExist()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");
        request.Content = new StringContent("{\"fileLocation\":\"/invalid/path\"}", Encoding.UTF8, "application/json");

        // Act
        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        var parsedJson = JObject.Parse(responseContent);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Failed to load a pdf.", parsedJson["errors"]["901"][0]);
    }

    [Fact]
    public async Task AnalyzeRequestHandler_ShouldReturnResult_WhenFileExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");
        request.Content = new StringContent("{\"fileLocation\":\"https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf\"}", Encoding.UTF8, "application/json");

        var responseContent = await request.Content.ReadAsStringAsync();
        // Act
        var response = await client.SendAsync(request);
        responseContent = await response.Content.ReadAsStringAsync();
        var parsedJson = JObject.Parse(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf", parsedJson["url"]);
        Assert.Equal(false, parsedJson["containsAnonymizedData"]);
        Assert.Equal(0, parsedJson["anonymizedPercentage"]);
        Assert.Equal(1, parsedJson["pageCount"]);
        Assert.Equal(0, parsedJson["anonymizedPercentagePerPage"]["1"]);
        Assert.Empty(parsedJson["originalImages"]);
        Assert.Empty(parsedJson["resultImages"]);
    }

    [Fact]
    public async Task AnalyzeRequestHandler_ShouldReturnResultWithImages_WhenReturnImagesIsTrue()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze");
        request.Content = new StringContent("{\"fileLocation\":\"https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf\", \"returnImages\": true}", Encoding.UTF8, "application/json");

        // Act
        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        var parsedJson = JObject.Parse(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf", parsedJson["url"]);
        Assert.Equal(false, parsedJson["containsAnonymizedData"]);
        Assert.Equal(0, parsedJson["anonymizedPercentage"]);
        Assert.Equal(1, parsedJson["pageCount"]);
        Assert.Equal(0, parsedJson["anonymizedPercentagePerPage"]["1"]);
        Assert.Single(parsedJson["originalImages"]);
        Assert.Single(parsedJson["resultImages"]);
    }
}