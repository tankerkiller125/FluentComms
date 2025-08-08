using System.Threading.Tasks;
using FluentComms.Core.Configuration;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using Xunit;

namespace FluentCommunications.Core.Tests
{
    /// <summary>
    /// Final integration test to verify the exact syntax from the problem statement works
    /// </summary>
    public class FinalIntegrationTest
    {
        private class TestProvider : IProvider
        {
            public bool WasCalled { get; private set; }
            public IMessage? LastMessage { get; private set; }

            public async Task<ISenderResult> SendAsync(IMessage message)
            {
                WasCalled = true;
                LastMessage = message;
                return new SenderResult { Successful = true };
            }

            public ISenderResult Send(IMessage message)
            {
                return SendAsync(message).Result;
            }
        }

        [Fact]
        public async Task Email_ExactProblemStatementSyntax_WorksCorrectly()
        {
            // Arrange
            var provider = new TestProvider();
            FluentCommsConfiguration.DefaultEmailProvider = provider;

            try
            {
                // Act - This is the EXACT syntax from the problem statement
                var email = await global::FluentComms.FluentComms.Email()
                    .From("john@email.com")
                    .To("bob@email.com", "bob")
                    .Subject("hows it going bob")
                    .Body("yo bob, long time no see!")
                    .SendAsync();

                // Assert
                Assert.True(email.Successful);
                Assert.True(provider.WasCalled);
                
                var sentEmail = provider.LastMessage as IEmail;
                Assert.NotNull(sentEmail);
                Assert.Equal("john@email.com", sentEmail.From?.Email);
                Assert.Single(sentEmail.To);
                Assert.Equal("bob@email.com", sentEmail.To[0].Email);
                Assert.Equal("bob", sentEmail.To[0].Name);
                Assert.Equal("hows it going bob", sentEmail.Subject);
                Assert.Equal("yo bob, long time no see!", sentEmail.Body);
            }
            finally
            {
                FluentCommsConfiguration.DefaultEmailProvider = null;
            }
        }

        [Fact]
        public async Task Sms_ExactProblemStatementSyntax_WorksCorrectly()
        {
            // Arrange
            var provider = new TestProvider();
            FluentCommsConfiguration.DefaultSmsProvider = provider;

            try
            {
                // Act - This is the EXACT syntax from the problem statement
                var sms = await global::FluentComms.FluentComms.Sms()
                    .From("+15555554444")
                    .To("+15555552222")
                    .Body("yo bob, long time no see!")
                    .SendAsync();

                // Assert
                Assert.True(sms.Successful);
                Assert.True(provider.WasCalled);
                
                var sentSms = provider.LastMessage as ISms;
                Assert.NotNull(sentSms);
                Assert.Equal("+15555554444", sentSms.From);
                Assert.Equal("+15555552222", sentSms.To);
                Assert.Equal("yo bob, long time no see!", sentSms.Message);
            }
            finally
            {
                FluentCommsConfiguration.DefaultSmsProvider = null;
            }
        }
    }
}