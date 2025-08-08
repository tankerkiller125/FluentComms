using System.Threading.Tasks;
using FluentComms.Core;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using FluentComms.Core.Configuration;
using Moq;
using Xunit;

// Bring the root FluentComms namespace into scope
using FluentComms;

namespace FluentCommunications.Core.Tests
{
    public class FluentApiTests
    {
        [Fact]
        public void Email_FluentApi_BuildsCorrectEmail_RootNamespace()
        {
            // Act - Using the root FluentComms namespace
            var emailBuilder = global::FluentComms.FluentComms.Email()
                .From("john@email.com")
                .To("bob@email.com", "bob")
                .Subject("hows it going bob")
                .Body("yo bob, long time no see!");

            var email = emailBuilder.Build();

            // Assert
            Assert.NotNull(email.From);
            Assert.Equal("john@email.com", email.From.Email);
            Assert.Single(email.To);
            Assert.Equal("bob@email.com", email.To[0].Email);
            Assert.Equal("bob", email.To[0].Name);
            Assert.Equal("hows it going bob", email.Subject);
            Assert.Equal("yo bob, long time no see!", email.Body);
        }

        [Fact]
        public void Sms_FluentApi_BuildsCorrectSms_RootNamespace()
        {
            // Act - Using the root FluentComms namespace
            var smsBuilder = global::FluentComms.FluentComms.Sms()
                .From("+15555554444")
                .To("+15555552222")
                .Body("yo bob, long time no see!");

            var sms = smsBuilder.Build();

            // Assert
            Assert.Equal("+15555554444", sms.From);
            Assert.Equal("+15555552222", sms.To);
            Assert.Equal("yo bob, long time no see!", sms.Message);
        }

        [Fact]
        public void Email_FluentApi_BuildsCorrectEmail()
        {
            // Act - Using the FluentComms.Core.FluentCommunications class
            var emailBuilder = FluentComms.Core.FluentCommunications.Email()
                .From("john@email.com")
                .To("bob@email.com", "bob")
                .Subject("hows it going bob")
                .Body("yo bob, long time no see!");

            var email = emailBuilder.Build();

            // Assert
            Assert.NotNull(email.From);
            Assert.Equal("john@email.com", email.From.Email);
            Assert.Single(email.To);
            Assert.Equal("bob@email.com", email.To[0].Email);
            Assert.Equal("bob", email.To[0].Name);
            Assert.Equal("hows it going bob", email.Subject);
            Assert.Equal("yo bob, long time no see!", email.Body);
        }

        [Fact]
        public void Sms_FluentApi_BuildsCorrectSms()
        {
            // Act - Using the FluentComms.Core.FluentCommunications class
            var smsBuilder = FluentComms.Core.FluentCommunications.Sms()
                .From("+15555554444")
                .To("+15555552222")
                .Body("yo bob, long time no see!");

            var sms = smsBuilder.Build();

            // Assert
            Assert.Equal("+15555554444", sms.From);
            Assert.Equal("+15555552222", sms.To);
            Assert.Equal("yo bob, long time no see!", sms.Message);
        }

        [Fact]
        public async Task Email_SendAsync_CallsProvider()
        {
            // Arrange
            var mockProvider = new Mock<IProvider>();
            var expectedResult = new SenderResult { Successful = true };
            mockProvider.Setup(p => p.SendAsync(It.IsAny<IMessage>())).ReturnsAsync(expectedResult);

            FluentCommsConfiguration.DefaultEmailProvider = mockProvider.Object;

            try
            {
                // Act
                var result = await global::FluentComms.FluentComms.Email()
                    .From("john@email.com")
                    .To("bob@email.com", "bob")
                    .Subject("hows it going bob")
                    .Body("yo bob, long time no see!")
                    .SendAsync();

                // Assert
                Assert.True(result.Successful);
                mockProvider.Verify(p => p.SendAsync(It.IsAny<IEmail>()), Times.Once);
            }
            finally
            {
                // Cleanup
                FluentCommsConfiguration.DefaultEmailProvider = null;
            }
        }

        [Fact]
        public async Task Sms_SendAsync_CallsProvider()
        {
            // Arrange
            var mockProvider = new Mock<IProvider>();
            var expectedResult = new SenderResult { Successful = true };
            mockProvider.Setup(p => p.SendAsync(It.IsAny<IMessage>())).ReturnsAsync(expectedResult);

            // Ensure clean state
            FluentCommsConfiguration.DefaultSmsProvider = null;
            FluentCommsConfiguration.DefaultSmsProvider = mockProvider.Object;

            try
            {
                // Act
                var result = await global::FluentComms.FluentComms.Sms()
                    .From("+15555554444")
                    .To("+15555552222")
                    .Body("yo bob, long time no see!")
                    .SendAsync();

                // Assert
                Assert.True(result.Successful);
                mockProvider.Verify(p => p.SendAsync(It.IsAny<ISms>()), Times.Once);
            }
            finally
            {
                // Cleanup
                FluentCommsConfiguration.DefaultSmsProvider = null;
            }
        }

        [Fact]
        public void Email_SendAsync_ThrowsWhenNoProviderConfigured()
        {
            // Arrange
            FluentCommsConfiguration.DefaultEmailProvider = null;

            var emailBuilder = global::FluentComms.FluentComms.Email()
                .From("john@email.com")
                .To("bob@email.com", "bob")
                .Subject("hows it going bob")
                .Body("yo bob, long time no see!");

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await emailBuilder.SendAsync());
            Assert.Contains("No email provider configured", exception.Result.Message);
        }

        [Fact]
        public void Sms_SendAsync_ThrowsWhenNoProviderConfigured()
        {
            // Arrange
            FluentCommsConfiguration.DefaultSmsProvider = null;

            var smsBuilder = global::FluentComms.FluentComms.Sms()
                .From("+15555554444")
                .To("+15555552222")
                .Body("yo bob, long time no see!");

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await smsBuilder.SendAsync());
            Assert.Contains("No SMS provider configured", exception.Result.Message);
        }
    }
}