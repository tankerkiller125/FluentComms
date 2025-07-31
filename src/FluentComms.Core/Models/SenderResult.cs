using System.Collections.Generic;
using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Models
{
    public class SenderResult : ISenderResult
    {
        public bool Successful { get; set; }

        public List<string> Errors { get; } = new List<string>();
    }
}
