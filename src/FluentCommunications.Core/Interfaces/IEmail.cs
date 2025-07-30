using System.Collections.Generic;

namespace FluentCommunications.Core.Interfaces
{
    public interface IEmail : IMessage
    {
        IAddress? From { get; set; }

        List<IAddress> To { get; }

        List<IAddress> Cc { get; }

        List<IAddress> Bcc { get; }

        string? Subject { get; set; }

        string? Body { get; set; }
    }
}
