using FluentCommunications.Core.Channels;
using FluentCommunications.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCommunications.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentCommunicationsCore(this IServiceCollection services)
        {
            services.AddScoped<IChannel<IEmail>, EmailChannel>();
            return services;
        }
    }
}
