using Microsoft.Extensions.DependencyInjection;
using FluentComms.Core.Interfaces;

namespace FluentComms.Providers.Liquid.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLiquidTemplateProvider(this IServiceCollection services)
        {
            services.AddSingleton<ITemplateProvider, LiquidTemplateProvider>();
            return services;
        }
    }
}
