var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PDFAnalyzer>();
var app = builder.Build();
app.MapPost("/analyze", RequestsHandlerService.AnalyzeRequestHandler);
app.Run();