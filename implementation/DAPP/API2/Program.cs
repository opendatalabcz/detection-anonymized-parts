using API2.Common.Errors;
using Application;
using Infrastructure;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API2;

/// <summary>
/// Program.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point to the API application.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        {
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
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

            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DappDbContext>();
            context.Database.Migrate();

            app.Run();
        }
    }
}
