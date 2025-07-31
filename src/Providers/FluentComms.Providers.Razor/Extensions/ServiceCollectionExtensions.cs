using Microsoft.Extensions.DependencyInjection;
using FluentComms.Core.Interfaces;

namespace FluentComms.Providers.Razor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRazorTemplateProvider(this IServiceCollection services)
        {
            services.AddSingleton<ITemplateProvider, RazorTemplateProvider>();
            return services;
        }
    }
}
