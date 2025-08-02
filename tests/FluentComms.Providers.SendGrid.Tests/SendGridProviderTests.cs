using System.Threading.Tasks;
using FluentComms.Core.Interfaces;
using FluentComms.Renderers.SendGrid;
using Moq;
using Xunit;

namespace FluentComms.Renderers.SendGrid.Tests
{
    public class SendGridProviderTests
    {
        [Fact]
        public async Task SendAsync_WithMessageNotEmail_ReturnsFailure()
        {
            // Arrange
            var provider = new SendGridProvider("dummy-api-key");
            var message = new Mock<IMessage>();

            // Act
            var result = await provider.SendAsync(message.Object);

            // Assert
            Assert.False(result.Successful);
            Assert.Contains("SendGridProvider can only send emails.", result.Errors);
        }

        [Fact]
        public async Task SendAsync_WithEmailFromNull_ReturnsFailure()
        {
            // Arrange
            var provider = new SendGridProvider("dummy-api-key");
            var email = new Mock<IEmail>();
            email.Setup(e => e.From).Returns((IAddress)null);

            // Act
            var result = await provider.SendAsync(email.Object);

            // Assert
            Assert.False(result.Successful);
            Assert.Contains("From address is required.", result.Errors);
        }

        [Fact]
        public void Send_WithMessageNotEmail_ReturnsFailure()
        {
            // Arrange
            var provider = new SendGridProvider("dummy-api-key");
            var message = new Mock<IMessage>();

            // Act
            var result = provider.Send(message.Object);

            // Assert
            Assert.False(result.Successful);
            Assert.Contains("SendGridProvider can only send emails.", result.Errors);
        }

        [Fact]
        public void Send_WithEmailFromNull_ReturnsFailure()
        {
            // Arrange
            var provider = new SendGridProvider("dummy-api-key");
            var email = new Mock<IEmail>();
            email.Setup(e => e.From).Returns((IAddress)null);

            // Act
            var result = provider.Send(email.Object);

            // Assert
            Assert.False(result.Successful);
            Assert.Contains("From address is required.", result.Errors);
        }
    }
}
