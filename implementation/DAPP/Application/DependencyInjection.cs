using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    /// <summary>
    /// Dependency injection for the application layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the application layer to the service collection.
        /// </summary>
        /// <param name="services"> The service collection</param>
        /// <returns> The service collection</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}