using System.Threading.Tasks;
using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Templates
{
    public abstract class BaseTemplateProvider : ITemplateProvider
    {
        public abstract Task<string> RenderAsync(string template, object model);

        public virtual Task<string> RenderAsync<T>(string template, T model)
        {
            return RenderAsync(template, (object)model);
        }
    }
}
