using System.Collections.Generic;
using System.Linq;

namespace FluentComms.Core.Models
{
    /// <summary>
    /// Represents the response from a send operation.
    /// </summary>
    public class SendResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the communication was sent successfully.
        /// </summary>
        public bool Successful { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the message, as provided by the communication provider.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets the list of error messages encountered during the send attempt.
        /// </summary>
        public List<string> ErrorMessages { get; } = new List<string>();

        /// <summary>
        /// Gets a value indicating whether there were any errors.
        /// </summary>
        public bool HasErrors => ErrorMessages.Any();
    }
}
