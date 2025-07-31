using System.Collections.Generic;

namespace FluentComms.Core.Interfaces
{
    public interface ISenderResult
    {
        bool Successful { get; }

        List<string> Errors { get; }
    }
}
