using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a template renderer.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Parses a template string, replacing placeholders with values from a model.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="template">The template string to parse.</param>
        /// <param name="model">The model containing the data for the template.</param>
        /// <param name="isHtml">A flag indicating if the template produces HTML content.</param>
        /// <returns>A task that represents the asynchronous parse operation. The task result contains the parsed string.</returns>
        Task<string> ParseAsync<T>(string template, T model, bool isHtml);
    }
}
