using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using FluentComms.Core.Channels;
using FluentComms.Core.Configuration;

namespace FluentComms.Core.Builders
{
    public class SmsBuilder
    {
        private readonly Sms _sms;
        private IChannel<ISms>? _channel;

        public SmsBuilder()
        {
            _sms = new Sms();
        }

        public SmsBuilder(IChannel<ISms> channel)
        {
            _sms = new Sms();
            _channel = channel;
        }

        public SmsBuilder From(string phoneNumber)
        {
            _sms.From = phoneNumber;
            return this;
        }

        public SmsBuilder To(string phoneNumber)
        {
            _sms.To = phoneNumber;
            return this;
        }

        public SmsBuilder Body(string message)
        {
            _sms.Message = message;
            return this;
        }

        public async Task<ISenderResult> SendAsync()
        {
            var channel = GetChannel();
            return await channel.SendAsync(_sms);
        }

        public ISenderResult Send()
        {
            var channel = GetChannel();
            return channel.Send(_sms);
        }

        private IChannel<ISms> GetChannel()
        {
            if (_channel != null)
                return _channel;
                
            if (FluentCommsConfiguration.DefaultSmsProvider != null)
                return new SmsChannel(FluentCommsConfiguration.DefaultSmsProvider);
                
            throw new InvalidOperationException("No SMS provider configured. Please configure a default SMS provider using FluentCommsConfiguration.DefaultSmsProvider or inject a channel through the constructor.");
        }

        // Implicit conversion to Sms for backward compatibility
        public static implicit operator Sms(SmsBuilder builder)
        {
            return builder._sms;
        }

        // Get the underlying SMS object
        public Sms Build()
        {
            return _sms;
        }
    }
}