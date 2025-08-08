using FluentComms.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FluentComms.Core.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for setting up FluentComms in an IServiceCollection.
    /// </summary>
    public static class FluentCommsServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the FluentComms services to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configure">An action to configure the FluentCommsBuilder.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddFluentComms(this IServiceCollection services, Action<FluentCommsBuilder> configure)
        {
            var builder = new FluentCommsBuilder(services);
            configure(builder);

            // Register core services
            services.AddScoped<ICommunicationFactory, CommunicationFactory>();
            services.AddTransient<ICommunication>(sp => sp.GetRequiredService<ICommunicationFactory>().Create());

            // Register Composite Renderer, resolving its dependencies from the builder
            services.AddScoped<IRenderer, CompositeRenderer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<CompositeRenderer>>();
                var renderers = builder.RendererTypes.Select(t => (IRenderer)sp.GetRequiredService(t));
                return new CompositeRenderer(renderers, logger);
            });

            // Register Composite Sender, resolving its dependencies from the builder
            services.AddScoped<ISender, CompositeSender>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<CompositeSender>>();
                var senders = builder.SenderTypes.Select(t => (ISender)sp.GetRequiredService(t));
                return new CompositeSender(senders, logger);
            });

            return services;
        }
    }
}
