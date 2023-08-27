using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    /// <summary>
    /// Dependency injection for the infrastructure layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Setup infrastructure services, such as database context, repositories, etc.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddDbContext<DappDbContext>();
            services.AddPersistance();
            services.AddServices();
            return services;
        }

        /// <summary>
        /// Add persistance services, such as repositories.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPersistance(this IServiceCollection services)
        {

            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            return services;
        }

        /// <summary>
        /// Add services, such as file handling, date time provider, etc.
        /// </summary>
        /// <param name="services"> The service collection</param>
        /// <returns> The service collection</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileHandleService, FileHandleService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }
    }
}