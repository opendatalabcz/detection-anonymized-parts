using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Setup infrastructure services, such as database context, repositories, etc.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DappDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileHandleService, FileHandleService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }
    }
}