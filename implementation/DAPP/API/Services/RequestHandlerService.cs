namespace API.Services;

public static class RequestsHandlerService
{
    public static async Task AnalyzeRequestHandler(HttpContext context)
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

        // The contract name is the last part of the path (e.g. "*\contract1.pdf")
        var contractName = request.FileLocation.Split("/").Last().Split(".").First();
        // spracovanie súboru
        var pdf = await DappPDF.Create(fileBytes.Value, contractName, request.FileLocation);
        var result = await PDFAnalyzer.AnalyzeAsync(pdf, returnImages: request.ReturnImages);

        // vrátenie výsledku vo formáte JSON
        await context.Response.WriteAsJsonAsync(result);
    }
}
