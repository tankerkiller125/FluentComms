using FluentComms.Core.Interfaces;
using FluentComms.Renderers.SendGrid;
using Microsoft.Extensions.DependencyInjection;

namespace FluentComms.Renderers.SendGrid.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGrid(this IServiceCollection services, string apiKey)
        {
            services.AddSingleton<IProvider>(new SendGridProvider(apiKey));
            return services;
        }
    }
}
