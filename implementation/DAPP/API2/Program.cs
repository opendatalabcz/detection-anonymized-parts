using API2;
using API2.Common.Errors;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    _ = builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure();

    _ = builder.Services.AddControllers();

    _ = builder.Services.AddSingleton<ProblemDetailsFactory, DappProblemDetailsFactory>();
}

WebApplication app = builder.Build();
{
    _ = app
        .UseExceptionHandler("/error")
        .UseHttpsRedirection();
    _ = app.MapControllers();

    app.Run();
}