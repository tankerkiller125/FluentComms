using System.Diagnostics;

namespace FluentComms.Core.Models
{
    /// <summary>
    /// Represents a recipient's address for a communication message (e.g., email or phone number).
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public class Address
    {
        /// <summary>
        /// Gets or sets the display name associated with the address.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the actual address value (e.g., "test@example.com" or "+15551234567").
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        public Address() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class with a specified address value and optional name.
        /// </summary>
        /// <param name="value">The address value.</param>
        /// <param name="name">The display name.</param>
        public Address(string value, string name = null)
        {
            Value = value;
            Name = name;
        }
    }
}
