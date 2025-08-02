using Microsoft.Extensions.DependencyInjection;
using FluentComms.Core.Interfaces;
using Markdig;

namespace FluentComms.Renderers.Markdown.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMarkdownTemplateProvider(
            this IServiceCollection services,
            Action<MarkdownPipelineBuilder>? configurePipeline = null)
        {
            services.AddTransient<MarkdownTemplateProvider>(sp => new MarkdownTemplateProvider(configurePipeline));
            return services;
        }
    }
}
