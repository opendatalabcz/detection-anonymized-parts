

using API.Responses;

namespace API.Services;

public static class RequestsHandlerService
{
    public static async Task AnalyzeRequestHandler(HttpContext context)
    {
        // načítanie dát z requestu
        AnalyzeRequest? request = null;
        try
        {
            request = await context.Request!.ReadFromJsonAsync<AnalyzeRequest>()!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        if (request == null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        var fileBytes = await FileHandleService.GetBytes(request.FileLocation);

        if (fileBytes.IsError)
        {
            await context.Response.WriteAsJsonAsync(ApiErrors.LoadingPdfError);
            return;
        }

        // The contract name is the last part of the path (e.g. "*\contract1.pdf")
        var contractName = request.FileLocation.Split("/").Last().Split(".").First();
        // spracovanie súboru
        var pdf = await DappPDF.Create(fileBytes.Value, contractName, request.FileLocation);
        var result = await PDFAnalyzer.AnalyzeAsync(pdf, returnImages: request.ReturnImages);

        // vrátenie výsledku vo formáte JSON
        AnalyzeResponse response = new()
        {
            ContractName = result.ContractName,
            Url = result.Url,
            ContainsAnonymizedData = result.ContainsAnonymizedData,
            AnonymizedPercentage = result.AnonymizedPercentage,
            PageCount = result.PageCount,
            AnonymizedPercentagePerPage = result.AnonymizedPercentagePerPage,
            OriginalImages = result.OriginalImages,
            ResultImages = result.ResultImages
        };
        await context.Response.WriteAsJsonAsync(response);
    }
}
