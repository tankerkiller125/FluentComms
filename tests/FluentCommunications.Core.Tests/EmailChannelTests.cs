using System.Threading.Tasks;
using FluentCommunications.Core.Channels;
using FluentCommunications.Core.Interfaces;
using FluentCommunications.Core.Models;
using Moq;
using Xunit;

namespace FluentCommunications.Core.Tests
{
    public class EmailChannelTests
    {
        [Fact]
        public async Task SendAsync_WhenProviderSucceeds_ReturnsSuccess()
        {
            // Arrange
            var provider = new Mock<IProvider>();
            var email = new Mock<IEmail>();
            var expectedResult = new SenderResult { Successful = true };

            provider.Setup(p => p.SendAsync(email.Object)).ReturnsAsync(expectedResult);

            var channel = new EmailChannel(provider.Object);

            // Act
            var result = await channel.SendAsync(email.Object);

            // Assert
            Assert.True(result.Successful);
        }

        [Fact]
        public async Task SendAsync_WhenProviderFails_ReturnsFailure()
        {
            // Arrange
            var provider = new Mock<IProvider>();
            var email = new Mock<IEmail>();
            var expectedResult = new SenderResult { Successful = false };

            provider.Setup(p => p.SendAsync(email.Object)).ReturnsAsync(expectedResult);

            var channel = new EmailChannel(provider.Object);

            // Act
            var result = await channel.SendAsync(email.Object);

            // Assert
            Assert.False(result.Successful);
        }

        [Fact]
        public void Send_WhenProviderSucceeds_ReturnsSuccess()
        {
            // Arrange
            var provider = new Mock<IProvider>();
            var email = new Mock<IEmail>();
            var expectedResult = new SenderResult { Successful = true };

            provider.Setup(p => p.Send(email.Object)).Returns(expectedResult);

            var channel = new EmailChannel(provider.Object);

            // Act
            var result = channel.Send(email.Object);

            // Assert
            Assert.True(result.Successful);
        }

        [Fact]
        public void Send_WhenProviderFails_ReturnsFailure()
        {
            // Arrange
            var provider = new Mock<IProvider>();
            var email = new Mock<IEmail>();
            var expectedResult = new SenderResult { Successful = false };

            provider.Setup(p => p.Send(email.Object)).Returns(expectedResult);

            var channel = new EmailChannel(provider.Object);

            // Act
            var result = channel.Send(email.Object);

            // Assert
            Assert.False(result.Successful);
        }
    }
}
