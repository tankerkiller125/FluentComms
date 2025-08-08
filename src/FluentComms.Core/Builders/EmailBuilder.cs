using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using FluentComms.Core.Channels;
using FluentComms.Core.Configuration;

namespace FluentComms.Core.Builders
{
    public class EmailBuilder
    {
        private readonly Email _email;
        private IChannel<IEmail>? _channel;

        public EmailBuilder()
        {
            _email = new Email();
        }

        public EmailBuilder(IChannel<IEmail> channel)
        {
            _email = new Email();
            _channel = channel;
        }

        public EmailBuilder From(string emailAddress, string? name = null)
        {
            _email.From = new Address(emailAddress, name);
            return this;
        }

        public EmailBuilder From(IAddress address)
        {
            _email.From = address;
            return this;
        }

        public EmailBuilder To(string emailAddress, string? name = null)
        {
            _email.To.Add(new Address(emailAddress, name));
            return this;
        }

        public EmailBuilder To(IAddress address)
        {
            _email.To.Add(address);
            return this;
        }

        public EmailBuilder Subject(string subject)
        {
            _email.Subject = subject;
            return this;
        }

        public EmailBuilder Body(string body)
        {
            _email.Body = body;
            return this;
        }

        public EmailBuilder Cc(string emailAddress, string? name = null)
        {
            _email.Cc.Add(new Address(emailAddress, name));
            return this;
        }

        public EmailBuilder Bcc(string emailAddress, string? name = null)
        {
            _email.Bcc.Add(new Address(emailAddress, name));
            return this;
        }

        public async Task<ISenderResult> SendAsync()
        {
            var channel = GetChannel();
            return await channel.SendAsync(_email);
        }

        public ISenderResult Send()
        {
            var channel = GetChannel();
            return channel.Send(_email);
        }

        private IChannel<IEmail> GetChannel()
        {
            if (_channel != null)
                return _channel;
                
            if (FluentCommsConfiguration.DefaultEmailProvider != null)
                return new EmailChannel(FluentCommsConfiguration.DefaultEmailProvider);
                
            throw new InvalidOperationException("No email provider configured. Please configure a default email provider using FluentCommsConfiguration.DefaultEmailProvider or inject a channel through the constructor.");
        }

        // Implicit conversion to Email for backward compatibility
        public static implicit operator Email(EmailBuilder builder)
        {
            return builder._email;
        }

        // Get the underlying email object
        public Email Build()
        {
            return _email;
        }
    }
}