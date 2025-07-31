using System.Threading.Tasks;
using RazorLight;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Templates;

namespace FluentComms.Providers.Razor
{
    public class RazorTemplateProvider : BaseTemplateProvider
    {
        private readonly RazorLightEngine _razorEngine;

        public RazorTemplateProvider()
        {
            _razorEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(RazorTemplateProvider))
                .UseMemoryCachingProvider()
                .Build();
        }

        public override async Task<string> RenderAsync(string template, object model)
        {
            var templateKey = Guid.NewGuid().ToString();
            return await _razorEngine.CompileRenderStringAsync(templateKey, template, model);
        }
    }
}
