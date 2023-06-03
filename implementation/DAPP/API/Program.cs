using API.Requests;
using DAPPAnalyzer.Interfaces;
using DAPPAnalyzer.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Service registration
builder.Services.AddScoped<IPDFAnalyzer, PDFAnalyzer>();

// Swagger config
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DAPP API", Version = "v1" });
//});

var app = builder.Build();

app.MapPost("/analyze", async (HttpContext context) =>
{

    var request = await context.Request.ReadFromJsonAsync<FileLocationRequest>();

    if (request == null)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid request format");
        return;
    }

    string fileLocation = request.FileLocation;

    if (!Uri.IsWellFormedUriString(fileLocation, UriKind.RelativeOrAbsolute))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid file location format");
        return;
    }

    byte[] fileBytes;
    if (Uri.TryCreate(fileLocation, UriKind.Absolute, out var uriResult)
        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
    {
        try
        {
            var httpClient = new HttpClient();
            fileBytes = await httpClient.GetByteArrayAsync(uriResult);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Unable to download the file. Error: {e.Message}");
            return;
        }
    }
    else
    {
        if (!File.Exists(fileLocation))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("File does not exist at the provided location");
            return;
        }

        try
        {
            fileBytes = await File.ReadAllBytesAsync(fileLocation);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Unable to read the file. Error: {e.Message}");
            return;
        }
    }

    // spracovanie súboru
    var analyzer = new PDFAnalyzer();
    var result = await analyzer.AnalyzeAsync(fileBytes);

    // vrátenie výsledku vo formáte JSON
    await context.Response.WriteAsJsonAsync(result);
});

//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DAPP API v1");
//});

app.Run();