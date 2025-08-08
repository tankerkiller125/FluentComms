using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Configuration
{
    public static class FluentCommsConfiguration
    {
        public static IProvider? DefaultEmailProvider { get; set; }
        public static IProvider? DefaultSmsProvider { get; set; }
    }
}