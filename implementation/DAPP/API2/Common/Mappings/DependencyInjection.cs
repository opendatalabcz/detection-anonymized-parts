using Mapster;
using MapsterMapper;
using System.Reflection;

namespace API2.Common.Mappings
{
    /// <summary>
    /// Dependency injection for mappings.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the mappings to the service collection.
        /// </summary>
        /// <param name="services"> The service collection</param>
        /// <returns> The service collection</returns>
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            TypeAdapterConfig cfg = TypeAdapterConfig.GlobalSettings;
            cfg.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(cfg);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
