using System.Collections.Generic;
using FluentCommunications.Core.Interfaces;

namespace FluentCommunications.Core.Models
{
    public class Email : IEmail
    {
        public IAddress? From { get; set; }

        public List<IAddress> To { get; } = new List<IAddress>();

        public List<IAddress> Cc { get; } = new List<IAddress>();

        public List<IAddress> Bcc { get; } = new List<IAddress>();

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
