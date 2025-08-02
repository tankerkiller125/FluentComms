using System.Threading.Tasks;
using FluentComms.Core.Interfaces;
using FluentComms.Renderers.Azure;
using Moq;
using Xunit;

namespace FluentComms.Renderers.Azure.Tests
{
    public class AzureCommunicationServicesProviderTests
    {
        [Fact]
        public async Task SendAsync_WithMessageNotEmailOrSms_ReturnsFailure()
        {
            // Arrange
            var provider = new AzureCommunicationServicesProvider("endpoint=https://dummy.communication.azure.com/;accesskey=dummy");
            var message = new Mock<IMessage>();

            // Act
            var result = await provider.SendAsync(message.Object);

            // Assert
            Assert.False(result.Successful);
            Assert.Contains("Unsupported message type.", result.Errors);
        }

        [Fact]
        public async Task SendAsync_WithEmailFromNull_ReturnsFailure()
        {
            // Arrange
            var provider = new AzureCommunicationServicesProvider("endpoint=https://dummy.communication.azure.com/;accesskey=dummy");
            var email = new Mock<IEmail>();
            email.Setup(e => e.From).Returns((IAddress)null);

            // Act
            var result = await provider.SendAsync(email.Object);

            // Assert
            Assert.False(result.Successful);
            Assert.Contains("From address is required.", result.Errors);
        }

        [Fact]
        public void Send_WithMessageNotEmailOrSms_ReturnsFailure()
        {
            // Arrange
            var provider = new AzureCommunicationServicesProvider("endpoint=https://dummy.communication.azure.com/;accesskey=dummy");
            var message = new Mock<IMessage>();

            // Act
            var result = provider.Send(message.Object);

            // Assert
            Assert.False(result.Successful);
            Assert.Contains("Unsupported message type.", result.Errors);
        }

        [Fact]
        public void Send_WithEmailFromNull_ReturnsFailure()
        {
            // Arrange
            var provider = new AzureCommunicationServicesProvider("endpoint=https://dummy.communication.azure.com/;accesskey=dummy");
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
