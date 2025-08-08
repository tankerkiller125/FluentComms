using Xunit;
using Moq;
using FluentComms.Core;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;

namespace FluentComms.Tests
{
    public class CompositeSenderTests
    {
        private readonly Mock<ILogger<CompositeSender>> _mockLogger;
        private readonly CommunicationMessage _testMessage;

        public CompositeSenderTests()
        {
            _mockLogger = new Mock<ILogger<CompositeSender>>();
            _testMessage = new CommunicationMessage();
        }

        [Fact]
        public async Task SendAsync_ShouldReturnSuccess_WhenFirstSenderSucceeds()
        {
            // Arrange
            var mockSender1 = new Mock<ISender>();
            var mockSender2 = new Mock<ISender>();
            var successfulResponse = new SendResponse { Successful = true, MessageId = "123" };

            mockSender1.Setup(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(successfulResponse);

            var senders = new[] { mockSender1.Object, mockSender2.Object };
            var compositeSender = new CompositeSender(senders, _mockLogger.Object);

            // Act
            var result = await compositeSender.SendAsync(_testMessage, CancellationToken.None);

            // Assert
            Assert.True(result.Successful);
            Assert.Equal("123", result.MessageId);
            mockSender1.Verify(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()), Times.Once);
            mockSender2.Verify(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task SendAsync_ShouldFallBackToSecondSender_WhenFirstFails()
        {
            // Arrange
            var mockSender1 = new Mock<ISender>();
            var mockSender2 = new Mock<ISender>();
            var failedResponse = new SendResponse { Successful = false, ErrorMessages = { "Failure" } };
            var successfulResponse = new SendResponse { Successful = true, MessageId = "456" };

            mockSender1.Setup(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(failedResponse);
            mockSender2.Setup(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(successfulResponse);

            var senders = new[] { mockSender1.Object, mockSender2.Object };
            var compositeSender = new CompositeSender(senders, _mockLogger.Object);

            // Act
            var result = await compositeSender.SendAsync(_testMessage, CancellationToken.None);

            // Assert
            Assert.True(result.Successful);
            Assert.Equal("456", result.MessageId);
            mockSender1.Verify(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()), Times.Once);
            mockSender2.Verify(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendAsync_ShouldReturnAggregatedErrors_WhenAllSendersFail()
        {
            // Arrange
            var mockSender1 = new Mock<ISender>();
            var mockSender2 = new Mock<ISender>();
            var failedResponse1 = new SendResponse { Successful = false, ErrorMessages = { "Error 1" } };
            var failedResponse2 = new SendResponse { Successful = false, ErrorMessages = { "Error 2" } };

            mockSender1.Setup(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(failedResponse1);
            mockSender2.Setup(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(failedResponse2);

            var senders = new[] { mockSender1.Object, mockSender2.Object };
            var compositeSender = new CompositeSender(senders, _mockLogger.Object);

            // Act
            var result = await compositeSender.SendAsync(_testMessage, CancellationToken.None);

            // Assert
            Assert.False(result.Successful);
            Assert.Equal(2, result.ErrorMessages.Count);
            Assert.Contains("Error 1", result.ErrorMessages);
            Assert.Contains("Error 2", result.ErrorMessages);
        }

        [Fact]
        public async Task SendAsync_ShouldHandleSenderExceptionAndFallBack()
        {
            // Arrange
            var mockSender1 = new Mock<ISender>();
            var mockSender2 = new Mock<ISender>();
            var successfulResponse = new SendResponse { Successful = true, MessageId = "789" };

            mockSender1.Setup(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new InvalidOperationException("Connection lost"));
            mockSender2.Setup(s => s.SendAsync(_testMessage, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(successfulResponse);

            var senders = new[] { mockSender1.Object, mockSender2.Object };
            var compositeSender = new CompositeSender(senders, _mockLogger.Object);

            // Act
            var result = await compositeSender.SendAsync(_testMessage, CancellationToken.None);

            // Assert
            Assert.True(result.Successful);
            Assert.Equal("789", result.MessageId);
        }

        [Fact]
        public async Task SendAsync_ShouldReturnError_WhenNoSendersConfigured()
        {
            // Arrange
            var senders = Enumerable.Empty<ISender>();
            var compositeSender = new CompositeSender(senders, _mockLogger.Object);

            // Act
            var result = await compositeSender.SendAsync(_testMessage, CancellationToken.None);

            // Assert
            Assert.False(result.Successful);
            Assert.Single(result.ErrorMessages);
            Assert.Equal("No senders have been configured in the provider chain.", result.ErrorMessages.First());
        }
    }
}
