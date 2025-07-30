using System.Collections.Generic;
using FluentCommunications.Core.Interfaces;

namespace FluentCommunications.Core.Models
{
    public class SenderResult : ISenderResult
    {
        public bool Successful { get; set; }

        public List<string> Errors { get; } = new List<string>();
    }
}
