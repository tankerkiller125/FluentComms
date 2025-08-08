using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentComms.Providers.MailKit
{
    public class MailKitSmtpSender : ISender
    {
        private readonly MailKitSmtpOptions _options;
        private readonly ILogger<MailKitSmtpSender> _logger;

        public MailKitSmtpSender(IOptions<MailKitSmtpOptions> options, ILogger<MailKitSmtpSender> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<SendResponse> SendAsync(CommunicationMessage message, CancellationToken cancellationToken)
        {
            var response = new SendResponse();
            try
            {
                var mimeMessage = CreateMimeMessage(message);

                using (var client = new SmtpClient())
                {
                    _logger.LogDebug("Connecting to SMTP host {Host}:{Port}", _options.Host, _options.Port);
                    await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTlsWhenAvailable, cancellationToken).ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(_options.Username))
                    {
                        _logger.LogDebug("Authenticating with username {Username}", _options.Username);
                        await client.AuthenticateAsync(_options.Username, _options.Password, cancellationToken).ConfigureAwait(false);
                    }

                    _logger.LogInformation("Sending message from {From} to {ToCount} recipients.", message.FromAddress.Value, message.ToAddresses.Count);
                    await client.SendAsync(mimeMessage, cancellationToken).ConfigureAwait(false);

                    response.Successful = true;
                    response.MessageId = mimeMessage.MessageId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with MailKit.");
                response.Successful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        private MimeMessage CreateMimeMessage(CommunicationMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(message.FromAddress.Name, message.FromAddress.Value));
            mimeMessage.To.AddRange(message.ToAddresses.Select(a => new MailboxAddress(a.Name, a.Value)));
            mimeMessage.Cc.AddRange(message.CcAddresses.Select(a => new MailboxAddress(a.Name, a.Value)));
            mimeMessage.Bcc.AddRange(message.BccAddresses.Select(a => new MailboxAddress(a.Name, a.Value)));

            mimeMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();
            if (message.IsHtml)
                bodyBuilder.HtmlBody = message.Body;
            else
                bodyBuilder.TextBody = message.Body;

            foreach(var attachment in message.Attachments)
            {
                bodyBuilder.Attachments.Add(attachment.Filename, attachment.Data, ContentType.Parse(attachment.ContentType));
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            // Set priority
            switch(message.Priority)
            {
                case Priority.High:
                    mimeMessage.Priority = MessagePriority.Urgent;
                    break;
                case Priority.Low:
                    mimeMessage.Priority = MessagePriority.NonUrgent;
                    break;
                default:
                    mimeMessage.Priority = MessagePriority.Normal;
                    break;
            }

            return mimeMessage;
        }
    }
}
