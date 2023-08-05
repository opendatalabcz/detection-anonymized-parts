using Application.Common.Interfaces.Persistance;
using Application.Common.Interfaces.Services;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
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
            this IServiceCollection services)
        {
            services.AddDbContext<DappDbContext>(options =>
                options.UseSqlite());
            services.AddPersistance();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

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
    }
}