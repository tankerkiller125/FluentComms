using FluentComms.Core.Interfaces;
using Fluid;
using System.Threading.Tasks;

namespace FluentComms.Renderers.Liquid
{
    public class LiquidRenderer : IRenderer
    {
        private static readonly FluidParser _parser = new FluidParser();

        public Task<string> ParseAsync<T>(string template, T model, bool isHtml)
        {
            if (_parser.TryParse(template, out var fluidTemplate, out var error))
            {
                var context = new TemplateContext(model);
                // In Fluid, Html-encoding is on by default. We can disable it if the source is not meant to be Html.
                // context.Options.HtmlEncoder = isHtml ? System.Text.Encodings.Web.HtmlEncoder.Default : null; // This API does not exist in the version used.
                var result = fluidTemplate.RenderAsync(context).AsTask();
                return result;
            }
            else
            {
                // If the template is invalid, throw an exception with the details from the parser.
                throw new TemplateParseException($"Liquid template parsing failed: {error}");
            }
        }
    }
}
