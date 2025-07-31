using FluentComms.Core.Interfaces;
using FluentComms.Providers.SendGrid;
using Microsoft.Extensions.DependencyInjection;

namespace FluentComms.Providers.SendGrid.Extensions
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
