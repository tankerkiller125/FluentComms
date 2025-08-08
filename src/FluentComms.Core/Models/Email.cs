using FluentComms.Core.Interfaces;

namespace FluentComms.Core.Models
{
    public class Email : IEmail
    {
        public IAddress? From { get; set; }

        public List<IAddress> To { get; }

        public List<IAddress> Cc { get; } = new List<IAddress>();

        public List<IAddress> Bcc { get; } = new List<IAddress>();

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public Email()
        {
            To = new List<IAddress>();
        }

        // Fluent methods
        public Email SetFrom(IAddress from)
        {
            From = from;
            return this;
        }
        public Email AddTo(IAddress to)
        {
            To.Add(to);
            return this;
        }
        
        public Email AddTo(IEnumerable<string> to)
        {
            foreach (var address in to)
            {
                To.Add(new Address(address));
            }
            return this;
        }
        
        public Email AddCc(IAddress cc)
        {
            Cc.Add(cc);
            return this;
        }
        
        public Email AddCc(IEnumerable<string> cc)
        {
            foreach (var address in cc)
            {
                Cc.Add(new Address(address));
            }
            return this;
        }
        
        public Email AddBcc(IAddress bcc)
        {
            Bcc.Add(bcc);
            return this;
        }
        
        public Email AddBcc(IEnumerable<string> bcc)
        {
            foreach (var address in bcc)
            {
                Bcc.Add(new Address(address));
            }
            return this;
        }
        
        public Email SetSubject(string subject)
        {
            Subject = subject;
            return this;
        }
        
        public Email SetBody(string body)
        {
            Body = body;
            return this;
        }

        
        // Template support
        public async Task<Email> SetBodyFromTemplateAsync(ITemplateProvider templateProvider, string template, object model)
        {
            Body = await templateProvider.RenderAsync(template, model);
            return this;
        }

        public async Task<Email> SetBodyFromTemplateAsync<T>(ITemplateProvider templateProvider, string template, T model)
        {
            Body = await templateProvider.RenderAsync(template, model);
            return this;
        }

        public async Task<Email> SetSubjectFromTemplateAsync(ITemplateProvider templateProvider, string template, object model)
        {
            Subject = await templateProvider.RenderAsync(template, model);
            return this;
        }

        public async Task<Email> SetSubjectFromTemplateAsync<T>(ITemplateProvider templateProvider, string template, T model)
        {
            Subject = await templateProvider.RenderAsync(template, model);
            return this;
        }
    }
}
