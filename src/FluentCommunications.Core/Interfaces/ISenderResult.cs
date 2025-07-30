using System.Collections.Generic;

namespace FluentCommunications.Core.Interfaces
{
    public interface ISenderResult
    {
        bool Successful { get; }

        List<string> Errors { get; }
    }
}
