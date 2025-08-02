using FluentComms.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FluentComms.Renderers.Liquid.Extensions
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
