using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Models
{
    public class Sms : ISms
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }

        public Sms SetFrom(string from)
        {
            From = from;
            return this;
        }
        public Sms SetTo(string to)
        {
            To = to;
            return this;
        }
        public Sms SetMessage(string message)
        {
            Message = message;
            return this;
        }

        // Template support
        public async Task<Sms> SetMessageFromTemplateAsync(ITemplateProvider templateProvider, string template, object model)
        {
            Message = await templateProvider.RenderAsync(template, model);
            return this;
        }

        public async Task<Sms> SetMessageFromTemplateAsync<T>(ITemplateProvider templateProvider, string template, T model)
        {
            Message = await templateProvider.RenderAsync(template, model);
            return this;
        }
    }
}
