using API.Requests;
using API.Services;
using DAPPAnalyzer.Interfaces;
using DAPPAnalyzer.Models;
using DAPPAnalyzer.Services;

var builder = WebApplication.CreateBuilder(args);

// Service registration
builder.Services.AddScoped<IPDFAnalyzer, PDFAnalyzer>();

var app = builder.Build();

app.MapPost("/analyze", async (HttpContext context) =>
{
    // načítanie dát z requestu
    var request = await context.Request.ReadFromJsonAsync<FileLocationRequest>();
    if (request == null)
    {
        await context.Response.WriteAsJsonAsync(new { error = "Invalid request" });
        return;
    }
    var fileBytes = await FileHandleService.GetBytes(request.FileLocation);

    if (fileBytes.IsError)
    {
        await context.Response.WriteAsJsonAsync(new { error = fileBytes.FirstError });
        return;
    }

    // spracovanie súboru
    var analyzer = new PDFAnalyzer();
    var pdf = await DappPDF.Create(fileBytes.Value);
    var result = await analyzer.AnalyzeAsync(pdf);

    // vrátenie výsledku vo formáte JSON
    await context.Response.WriteAsJsonAsync(result);
});

app.Run();