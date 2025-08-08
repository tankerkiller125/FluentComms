using FluentComms.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentComms.Core
{
    /// <summary>
    /// An implementation of IRenderer that processes a template through a pipeline of other renderers.
    /// </summary>
    public class CompositeRenderer : IRenderer
    {
        private readonly IEnumerable<IRenderer> _renderers;
        private readonly ILogger<CompositeRenderer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeRenderer"/> class.
        /// </summary>
        /// <param name="renderers">The collection of renderers to form the pipeline.</param>
        /// <param name="logger">The logger instance.</param>
        public CompositeRenderer(IEnumerable<IRenderer> renderers, ILogger<CompositeRenderer> logger)
        {
            _renderers = renderers ?? Enumerable.Empty<IRenderer>();
            _logger = logger;
        }

        /// <summary>
        /// Parses a template by passing it through a sequential pipeline of registered renderers.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="template">The initial template string.</param>
        /// <param name="model">The model data for the template.</param>
        /// <param name="isHtml">A flag indicating if the content is HTML.</param>
        /// <returns>The final processed string after all rendering stages.</returns>
        public async Task<string> ParseAsync<T>(string template, T model, bool isHtml)
        {
            if (!_renderers.Any())
            {
                _logger.LogWarning("No renderers are configured. Returning original template content.");
                return template;
            }

            var processedTemplate = template;

            foreach (var renderer in _renderers)
            {
                try
                {
                    _logger.LogDebug("Applying renderer {RendererType}", renderer.GetType().Name);
                    processedTemplate = await renderer.ParseAsync(processedTemplate, model, isHtml).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying renderer {RendererType}", renderer.GetType().Name);
                    // We re-throw here because a failure in the rendering pipeline is a critical failure.
                    // The user should know their template is broken.
                    throw;
                }
            }

            _logger.LogInformation("Template rendering pipeline completed successfully.");
            return processedTemplate;
        }
    }
}
