using FluentComms.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FluentComms.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a communication message sender.
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Sends a communication message asynchronously.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous send operation. The task result contains the provider's response.</returns>
        Task<SendResponse> SendAsync(CommunicationMessage message, CancellationToken cancellationToken);
    }
}
