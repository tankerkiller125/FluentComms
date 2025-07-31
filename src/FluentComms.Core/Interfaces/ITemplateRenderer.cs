using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    public interface ITemplateRenderer
    {
        Task<string> RenderTemplateAsync(string templateName, object model);
        Task<string> RenderTemplateAsync<T>(string templateName, T model);
        Task<string> RenderStringAsync(string templateContent, object model);
        Task<string> RenderStringAsync<T>(string templateContent, T model);
    }
}
