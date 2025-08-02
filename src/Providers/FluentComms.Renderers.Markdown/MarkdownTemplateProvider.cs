using Markdig;
using FluentComms.Core.Templates;

namespace FluentComms.Renderers.Markdown
{
    public class MarkdownTemplateProvider : BaseTemplateProvider
    {
        private readonly MarkdownPipeline _pipeline;

        public MarkdownTemplateProvider(Action<MarkdownPipelineBuilder>? configurePipeline = null)
        {
            var builder = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions();

            configurePipeline?.Invoke(builder);

            _pipeline = builder.Build();
        }

        public override Task<string> RenderAsync(string template, object model)
        {
            // For Markdown, we don't process the model - we just convert markdown to HTML
            // The model processing would be handled by subsequent providers in the chain
            var html = Markdig.Markdown.ToHtml(template, _pipeline);
            return Task.FromResult(html);
        }
    }
}
