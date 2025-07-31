using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    public interface ITemplateProvider
    {
        Task<string> RenderAsync(string template, object model);
        Task<string> RenderAsync<T>(string template, T model);
    }
}
