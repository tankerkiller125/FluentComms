using System.Threading.Tasks;
using Fluid;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Templates;

namespace FluentComms.Providers.Liquid
{
    public class LiquidTemplateProvider : BaseTemplateProvider
    {
        private readonly FluidParser _parser;

        public LiquidTemplateProvider()
        {
            _parser = new FluidParser();
        }

        public override async Task<string> RenderAsync(string template, object model)
        {
            if (_parser.TryParse(template, out var fluidTemplate, out var error))
            {
                var context = new TemplateContext(model);
                return await fluidTemplate.RenderAsync(context);
            }
            
            throw new InvalidOperationException($"Failed to parse Liquid template: {error}");
        }
    }
}
