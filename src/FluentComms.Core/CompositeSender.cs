using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentComms.Core
{
    /// <summary>
    /// An implementation of ISender that attempts to send a message through a chain of other senders,
    /// falling back to the next one upon failure.
    /// </summary>
    public class CompositeSender : ISender
    {
        private readonly IEnumerable<ISender> _senders;
        private readonly ILogger<CompositeSender> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSender"/> class.
        /// </summary>
        /// <param name="senders">The collection of senders to form the fallback chain.</param>
        /// <param name="logger">The logger instance.</param>
        public CompositeSender(IEnumerable<ISender> senders, ILogger<CompositeSender> logger)
        {
            _senders = senders ?? Enumerable.Empty<ISender>();
            _logger = logger;
        }

        /// <summary>
        /// Attempts to send a message using the registered senders in order, until one succeeds.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The response from the first successful sender, or an aggregated error response if all fail.</returns>
        public async Task<SendResponse> SendAsync(CommunicationMessage message, CancellationToken cancellationToken)
        {
            if (!_senders.Any())
            {
                _logger.LogError("No senders are configured. Cannot send message.");
                var response = new SendResponse { Successful = false };
                response.ErrorMessages.Add("No senders have been configured in the provider chain.");
                return response;
            }

            var aggregatedErrors = new List<string>();

            foreach (var sender in _senders)
            {
                try
                {
                    _logger.LogInformation("Attempting to send message with {SenderType}", sender.GetType().Name);
                    var response = await sender.SendAsync(message, cancellationToken).ConfigureAwait(false);

                    if (response.Successful)
                    {
                        _logger.LogInformation("Message sent successfully with {SenderType}. MessageId: {MessageId}", sender.GetType().Name, response.MessageId);
                        return response;
                    }

                    var errorMessage = $"Sender {sender.GetType().Name} failed: {string.Join(", ", response.ErrorMessages)}";
                    _logger.LogWarning(errorMessage);
                    aggregatedErrors.AddRange(response.ErrorMessages);
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Sender {sender.GetType().Name} threw an exception.";
                    _logger.LogError(ex, errorMessage);
                    aggregatedErrors.Add($"{errorMessage} Details: {ex.Message}");
                }
            }

            _logger.LogError("All configured senders failed to send the message.");
            var finalResponse = new SendResponse
            {
                Successful = false
            };
            finalResponse.ErrorMessages.AddRange(aggregatedErrors);
            return finalResponse;
        }
    }
}
