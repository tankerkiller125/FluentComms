using FluentComms.Core.Interfaces;
using System;

namespace FluentComms.Core
{
    /// <summary>
    /// The default implementation of the ICommunicationFactory interface.
    /// </summary>
    internal class CommunicationFactory : ICommunicationFactory
    {
        private readonly ISender _sender;
        private readonly IRenderer _renderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationFactory"/> class.
        /// </summary>
        /// <param name="sender">The sender service.</param>
        /// <param name="renderer">The renderer service.</param>
        public CommunicationFactory(ISender sender, IRenderer renderer)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        /// <summary>
        /// Creates a new communication message builder instance.
        /// </summary>
        /// <returns>A new instance of a class implementing ICommunication.</returns>
        public ICommunication Create()
        {
            return new Communication(_sender, _renderer);
        }
    }
}
