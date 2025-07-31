using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    public interface IChainableTemplateProvider : ITemplateProvider
    {
        IChainableTemplateProvider Chain(ITemplateProvider nextProvider);
        IChainableTemplateProvider Chain(params ITemplateProvider[] providers);
    }
}
