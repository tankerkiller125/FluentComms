using System.IO;

namespace FluentComms.Core.Models
{
    /// <summary>
    /// Represents a file attachment for a communication message.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the stream containing the file data.
        /// </summary>
        public Stream Data { get; set; }

        /// <summary>
        /// Gets or sets the MIME content type of the file (e.g., "application/pdf").
        /// </summary>
        public string ContentType { get; set; }
    }
}
