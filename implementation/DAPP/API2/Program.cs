using API2.Common.Errors;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API2;

public class Program
{
    public static void Main(string[] args)
    {
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
    }
}
