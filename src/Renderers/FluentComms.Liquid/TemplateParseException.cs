using System;

namespace FluentComms.Renderers.Liquid
{
    public class TemplateParseException : Exception
    {
        public TemplateParseException(string message) : base(message) { }
    }
}
