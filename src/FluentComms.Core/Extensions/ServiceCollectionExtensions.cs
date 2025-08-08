using FluentComms.Core.Channels;
using FluentComms.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluentComms.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFluentCommunicationsCore(this IServiceCollection services)
        {
            services.AddScoped<IChannel<IEmail>, EmailChannel>();
            services.AddScoped<IChannel<ISms>, SmsChannel>();
            return services;
        }
    }
}
