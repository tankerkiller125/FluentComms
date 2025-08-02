using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FluentComms.Renderers.SendGrid
{
    public class SendGridProvider : IProvider
    {
        private readonly SendGridClient _client;

        public SendGridProvider(string apiKey)
        {
            _client = new SendGridClient(apiKey);
        }

        public async Task<ISenderResult> SendAsync(IMessage message)
        {
            if (message is not IEmail email)
            {
                return new SenderResult
                {
                    Successful = false,
                    Errors = { "SendGridProvider can only send emails." }
                };
            }

            if (email.From == null)
            {
                return new SenderResult
                {
                    Successful = false,
                    Errors = { "From address is required." }
                };
            }

            var from = new EmailAddress(email.From.Email, email.From.Name);
            var tos = email.To.Select(a => new EmailAddress(a.Email, a.Name)).ToList();
            var subject = email.Subject;
            var plainTextContent = email.Body; // Assuming body is plain text for now
            var htmlContent = email.Body;

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent);

            if (email.Cc.Any())
            {
                msg.AddCcs(email.Cc.Select(a => new EmailAddress(a.Email, a.Name)).ToList());
            }

            if (email.Bcc.Any())
            {
                msg.AddBccs(email.Bcc.Select(a => new EmailAddress(a.Email, a.Name)).ToList());
            }

            var response = await _client.SendEmailAsync(msg);
            var result = new SenderResult
            {
                Successful = response.IsSuccessStatusCode
            };

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                result.Errors.Add(responseBody);
            }

            return result;
        }

        public ISenderResult Send(IMessage message)
        {
            return SendAsync(message).GetAwaiter().GetResult();
        }
    }
}
