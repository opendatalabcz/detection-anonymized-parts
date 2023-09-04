using Application.Analyzer.Commands.AnalyzeDocument;
using Application.Analyzer.Queries.GetAnalyzedDocument;
using Domain.DocumentAggregate;
using Domain.DocumentAggregate.ValueObjects;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
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
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze")
        {
            Content = new StringContent("invalid request", Encoding.UTF8, "application/json")
        };

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
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze")
        {
            Content = new StringContent("{\"fileLocation\":\"/invalid/path\"}", Encoding.UTF8, "application/json")
        };

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
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze")
        {
            Content = new StringContent("{\"fileLocation\":\"https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf\"}", Encoding.UTF8, "application/json")
        };
        _ = await request.Content.ReadAsStringAsync();
        // Act
        var response = await client.SendAsync(request);
        string? responseContent = await response.Content.ReadAsStringAsync();
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
    public async Task AnalyzeRequestHandler_ShouldReturnError_Validation()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze")
        {
            Content = new StringContent("{\"fileLocation\":\"\"}", Encoding.UTF8, "application/json")
        };

        // Act
        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        _ = JObject.Parse(responseContent);

        // Assert
        Assert.Equal((HttpStatusCode)400, response.StatusCode);
    }


    [Fact]
    public async Task AnalyzeRequestHandler_ShouldReturnResultWithImages_WhenReturnImagesIsTrue()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "/analyze")
        {
            Content = new StringContent("{\"fileLocation\":\"https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf\", \"returnImages\": true}", Encoding.UTF8, "application/json")
        };

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

    [Fact]
    public async Task GetDocumentPagesRequest_entity_not_found()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/results")
        {
            Content = new StringContent("{\"DocumentId\":\"12345678-1234-1234-1234-123456781234\"}", Encoding.UTF8, "application/json")
        };

        // Act
        var response = await client.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        var parsedJson = JObject.Parse(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("Entity with given Id is not in the database.", parsedJson["title"].Value<string>());
    }


    [Fact]
    public async Task AnalyzeDocumentHandler_ReturnError_EntityDoesNotExist()
    {
        // Arrange

        _ = _factory.CreateClient();
        var options = new DbContextOptionsBuilder<DappDbContext>()
            .Options;
        var context = new DappDbContext(options);
        var documentRepository = new DocumentRepository(context);
        var PageRepository = new PageRepository(context);
        var handler = new AnalyzeDocumentCommandHandler(PageRepository, documentRepository);
        // Act
        var r = await handler.Handle(new AnalyzeDocumentCommand(new(), DocumentId.CreateUnique(), false), CancellationToken.None);
        // Assert
        Assert.True(r.IsError);
        Assert.Equal(Domain.Common.Errors.Repository.EntityDoesNotExist, r.FirstError);
    }

    [Fact]
    public async Task GetAnalyzedDocumentHandler_ReturnError_DocumentNotYetAnalyzed()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DappDbContext>()
            .Options;
        var context = new DappDbContext(options);
        var DocumentRepository = new DocumentRepository(context);
        var handler = new GetAnalyzedDocumentDataQueryHandler(new FileHandleService(), DocumentRepository);
        var d = Document.Create("Test", "Test", "Test", new());
        var id = DocumentRepository.Add(d);

        // Act
        var r = await handler.Handle(new GetAnalyzedDocumentDataQuery(id), CancellationToken.None);

        // Assert
        Assert.True(r.IsError);
        Assert.Equal(Domain.Common.Errors.Analyzer.DocumentNotYetAnalyzed, r.FirstError);
    }
}
