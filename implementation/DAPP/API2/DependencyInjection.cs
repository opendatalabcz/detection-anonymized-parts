using API2.Common.Errors;
using API2.Common.Mappings;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API2
{
    /// <summary>
    /// Dependency injection.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the presentation layer to the service collection.
        /// </summary>
        /// <param name="services"> The service collection</param>
        /// <returns> The service collection</returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            _ = services.AddControllers();
            _ = services.AddSingleton<ProblemDetailsFactory, DappProblemDetailsFactory>();
            _ = services.AddMappings();
            return services;
        }
    }
}
