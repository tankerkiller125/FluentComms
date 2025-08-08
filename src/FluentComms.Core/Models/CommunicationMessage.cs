using System.Collections.Generic;

namespace FluentComms.Core.Models
{
    /// <summary>
    /// Represents the core communication message to be sent.
    /// </summary>
    public class CommunicationMessage
    {
        /// <summary>
        /// Gets or sets the sender's address.
        /// </summary>
        public Address FromAddress { get; set; }

        /// <summary>
        /// Gets the list of primary recipients.
        /// </summary>
        public List<Address> ToAddresses { get; } = new List<Address>();

        /// <summary>
        /// Gets the list of carbon copy (CC) recipients.
        /// </summary>
        public List<Address> CcAddresses { get; } = new List<Address>();

        /// <summary>
        /// Gets the list of blind carbon copy (BCC) recipients.
        /// </summary>
        public List<Address> BccAddresses { get; } = new List<Address>();

        /// <summary>
        /// Gets or sets the subject of the message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body of the message. This can be plain text, HTML, or a template.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the body content is HTML.
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Gets or sets the priority of the message.
        /// </summary>
        public Priority Priority { get; set; } = Priority.Normal;

        /// <summary>
        /// Gets the list of attachments for the message.
        /// </summary>
        public List<Attachment> Attachments { get; } = new List<Attachment>();

        /// <summary>
        /// Gets a dictionary for provider-specific data or metadata.
        /// </summary>
        public Dictionary<string, object> Tags { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the template model used for rendering the message body.
        /// </summary>
        internal object TemplateModel { get; set; }
    }
}
