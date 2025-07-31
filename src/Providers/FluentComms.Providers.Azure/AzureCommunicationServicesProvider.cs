using Azure;
using Azure.Communication.Email;
using Azure.Communication.Sms;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;

namespace FluentComms.Providers.Azure
{
    public class AzureCommunicationServicesProvider : IProvider
    {
        private readonly EmailClient _emailClient;
        private readonly SmsClient _smsClient;

        public AzureCommunicationServicesProvider(string connectionString)
        {
            _emailClient = new EmailClient(connectionString);
            _smsClient = new SmsClient(connectionString);
        }

        public async Task<ISenderResult> SendAsync(IMessage message)
        {
            if (message is IEmail email)
            {
                if (email.From == null)
                {
                    return new SenderResult
                    {
                        Successful = false,
                        Errors = { "From address is required." }
                    };
                }

                var emailContent = new EmailContent(email.Subject)
                {
                    PlainText = email.Body,
                    Html = email.Body
                };

                var toRecipients = email.To.Select(a => new EmailAddress(a.Email, a.Name)).ToList();
                var ccRecipients = email.Cc.Select(a => new EmailAddress(a.Email, a.Name)).ToList();
                var bccRecipients = email.Bcc.Select(a => new EmailAddress(a.Email, a.Name)).ToList();

                var emailRecipients = new EmailRecipients(toRecipients, ccRecipients, bccRecipients);

                var emailMessage = new EmailMessage(email.From.Email, emailRecipients, emailContent);

                try
                {
                    var emailSendOperation = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);
                    var result = new SenderResult { Successful = emailSendOperation.Value.Status == EmailSendStatus.Succeeded };
                    if (!result.Successful)
                    {
                        result.Errors.Add("Email send operation failed.");
                    }
                    return result;
                }
                catch (RequestFailedException ex)
                {
                    return new SenderResult { Successful = false, Errors = { ex.Message } };
                }
            }
            else if (message is ISms sms)
            {
                try
                {
                    var response = await _smsClient.SendAsync(sms.From, sms.To, sms.Message);
                    var result = new SenderResult { Successful = true };
                    // The SMS client does not provide immediate feedback on success or failure in the same way the email client does.
                    // We will assume success if no exception is thrown.
                    return result;
                }
                catch (Exception ex)
                {
                    return new SenderResult { Successful = false, Errors = { ex.Message } };
                }
            }

            return new SenderResult
            {
                Successful = false,
                Errors = { "Unsupported message type." }
            };
        }

        public ISenderResult Send(IMessage message)
        {
            return SendAsync(message).GetAwaiter().GetResult();
        }
    }
}
