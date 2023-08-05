using API2.Common.Errors;
using API2.Common.Mappings;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API2
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            _ = services.AddControllers();
            _ = services.AddSingleton<ProblemDetailsFactory, DappProblemDetailsFactory>();
            _ = services.AddMappings();
            return services;
        }
    }
}
