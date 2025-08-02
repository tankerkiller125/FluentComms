using FluentComms.Core.Templates;
using Fluid;
using Fluid.Values;

namespace FluentComms.Renderers.Liquid;

public class LiquidTemplateProvider : BaseTemplateProvider
{
    private readonly FluidParser _parser;
    private readonly TemplateOptions _options;

    public LiquidTemplateProvider()
    {
        _parser = new FluidParser();
        
        _options = new TemplateOptions();
        
        // Use UnsafeMemberAccessStrategy to allow access to all properties of the model
        var strategy = new UnsafeMemberAccessStrategy();
        _options.MemberAccessStrategy = strategy;
        
        // Map C# property names (PascalCase) to liquid template names (camelCase)
        strategy.MemberNameStrategy = MemberNameStrategies.CamelCase;
    }

    public override async Task<string> RenderAsync(string template, object model)
    {
        if (_parser.TryParse(template, out var fluidTemplate, out var error))
        {
            // Pass the model and options directly to the context
            var context = new TemplateContext(model, _options);
            
            return await fluidTemplate.RenderAsync(context);
        }
            
        throw new InvalidOperationException($"Failed to parse Liquid template: {error}");
    }
}