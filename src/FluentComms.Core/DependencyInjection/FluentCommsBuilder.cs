using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FluentComms.Core.DependencyInjection
{
    /// <summary>
    /// A builder for configuring FluentComms services.
    /// </summary>
    public class FluentCommsBuilder
    {
        /// <summary>
        /// Gets the service collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Gets the list of registered sender types.
        /// </summary>
        internal List<Type> SenderTypes { get; } = new List<Type>();

        /// <summary>
        /// Gets the list of registered renderer types.
        /// </summary>
        internal List<Type> RendererTypes { get; } = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCommsBuilder"/> class.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public FluentCommsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Adds a sender to the fallback chain.
        /// </summary>
        /// <typeparam name="T">The type of the sender, which must implement ISender.</typeparam>
        public FluentCommsBuilder AddSender<T>() where T : class, Interfaces.ISender
        {
            SenderTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Adds a renderer to the processing pipeline.
        /// </summary>
        /// <typeparam name="T">The type of the renderer, which must implement IRenderer.</typeparam>
        public FluentCommsBuilder AddRenderer<T>() where T : class, Interfaces.IRenderer
        {
            RendererTypes.Add(typeof(T));
            return this;
        }
    }
}
