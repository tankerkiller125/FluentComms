using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Templates
{
    public class ChainableTemplateProvider : IChainableTemplateProvider
    {
        private readonly List<ITemplateProvider> _providers;

        public ChainableTemplateProvider(ITemplateProvider firstProvider)
        {
            _providers = new List<ITemplateProvider> { firstProvider };
        }

        private ChainableTemplateProvider(List<ITemplateProvider> providers)
        {
            _providers = providers;
        }

        public IChainableTemplateProvider Chain(ITemplateProvider nextProvider)
        {
            var newProviders = new List<ITemplateProvider>(_providers) { nextProvider };
            return new ChainableTemplateProvider(newProviders);
        }

        public IChainableTemplateProvider Chain(params ITemplateProvider[] providers)
        {
            var newProviders = new List<ITemplateProvider>(_providers);
            newProviders.AddRange(providers);
            return new ChainableTemplateProvider(newProviders);
        }

        public async Task<string> RenderAsync(string template, object model)
        {
            string result = template;

            foreach (var provider in _providers)
            {
                result = await provider.RenderAsync(result, model);
            }

            return result;
        }

        public async Task<string> RenderAsync<T>(string template, T model)
        {
            string result = template;

            foreach (var provider in _providers)
            {
                result = await provider.RenderAsync(result, model);
            }

            return result;
        }

        public static IChainableTemplateProvider From(ITemplateProvider provider)
        {
            return new ChainableTemplateProvider(provider);
        }
    }
}
