using Microsoft.Extensions.DependencyInjection;
using FluentComms.Core.Interfaces;

namespace FluentComms.Providers.Markdown.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMarkdownTemplateProvider(this IServiceCollection services)
        {
            services.AddTransient<MarkdownTemplateProvider>();
            return services;
        }
    }
}
