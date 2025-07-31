using FluentComms.Core.Interfaces;
using FluentComms.Providers.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace FluentComms.Providers.Azure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureCommunicationServices(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IProvider>(new AzureCommunicationServicesProvider(connectionString));
            return services;
        }
    }
}
