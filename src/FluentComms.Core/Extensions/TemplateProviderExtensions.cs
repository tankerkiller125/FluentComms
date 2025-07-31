using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Extensions
{
    public static class TemplateProviderExtensions
    {
        /// <summary>
        /// Creates a chainable template provider from any template provider
        /// </summary>
        public static IChainableTemplateProvider AsChainable(this ITemplateProvider provider)
        {
            return Templates.ChainableTemplateProvider.From(provider);
        }

        /// <summary>
        /// Chains this template provider with another provider
        /// </summary>
        public static IChainableTemplateProvider Then(this ITemplateProvider provider, ITemplateProvider nextProvider)
        {
            return Templates.ChainableTemplateProvider.From(provider).Chain(nextProvider);
        }

        /// <summary>
        /// Chains this template provider with multiple providers
        /// </summary>
        public static IChainableTemplateProvider Then(this ITemplateProvider provider, params ITemplateProvider[] nextProviders)
        {
            return Templates.ChainableTemplateProvider.From(provider).Chain(nextProviders);
        }
    }
}
