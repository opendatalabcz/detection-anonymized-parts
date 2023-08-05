using Mapster;
using MapsterMapper;
using System.Reflection;

namespace API2.Common.Mappings
{
    public static class DependencyInjection
    {
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
