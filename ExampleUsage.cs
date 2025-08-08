using System;
using System.Linq;
using System.Threading.Tasks;
using FluentComms.Core.Configuration;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;

// Example demonstrating the exact syntax from the problem statement
public class ExampleUsage
{
    // Mock provider for demonstration
    public class MockEmailProvider : IProvider
    {
        public async Task<ISenderResult> SendAsync(IMessage message)
        {
            if (message is IEmail email)
            {
                Console.WriteLine($"Sending email from {email.From?.Email} to {string.Join(", ", email.To.Select(t => t.Email))}");
                Console.WriteLine($"Subject: {email.Subject}");
                Console.WriteLine($"Body: {email.Body}");
            }
            return new SenderResult { Successful = true };
        }

        public ISenderResult Send(IMessage message)
        {
            return SendAsync(message).Result;
        }
    }

    public class MockSmsProvider : IProvider
    {
        public async Task<ISenderResult> SendAsync(IMessage message)
        {
            if (message is ISms sms)
            {
                Console.WriteLine($"Sending SMS from {sms.From} to {sms.To}");
                Console.WriteLine($"Message: {sms.Message}");
            }
            return new SenderResult { Successful = true };
        }

        public ISenderResult Send(IMessage message)
        {
            return SendAsync(message).Result;
        }
    }

    public static async Task Main(string[] args)
    {
        // Configure providers
        FluentCommsConfiguration.DefaultEmailProvider = new MockEmailProvider();
        FluentCommsConfiguration.DefaultSmsProvider = new MockSmsProvider();

        // Example 1: Email - Exact syntax from problem statement
        var email = await FluentComms.FluentComms.Email()
            .From("john@email.com")
            .To("bob@email.com", "bob")
            .Subject("hows it going bob")
            .Body("yo bob, long time no see!")
            .SendAsync();

        Console.WriteLine($"Email sent successfully: {email.Successful}");

        // Example 2: SMS - Exact syntax from problem statement  
        var sms = await FluentComms.FluentComms.Sms()
            .From("+15555554444")
            .To("+15555552222")
            .Body("yo bob, long time no see!")
            .SendAsync();

        Console.WriteLine($"SMS sent successfully: {sms.Successful}");
    }
}