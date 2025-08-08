using System.Threading.Tasks;
using FluentComms.Core.Interfaces;
using FluentComms.Core.Models;
using FluentComms.Core.Configuration;
using Moq;
using Xunit;

namespace FluentCommunications.Core.Tests
{
    public class ProblemStatementTests
    {
        [Fact]
        public async Task Email_ExactSyntaxFromProblemStatement_Works()
        {
            // Arrange
            var mockProvider = new Mock<IProvider>();
            var expectedResult = new SenderResult { Successful = true };
            mockProvider.Setup(p => p.SendAsync(It.IsAny<IMessage>())).ReturnsAsync(expectedResult);

            FluentCommsConfiguration.DefaultEmailProvider = mockProvider.Object;

            try
            {
                // Act - This is the exact syntax from the problem statement
                var email = await global::FluentComms.FluentComms.Email()
                    .From("john@email.com")
                    .To("bob@email.com", "bob")
                    .Subject("hows it going bob")
                    .Body("yo bob, long time no see!")
                    .SendAsync();

                // Assert
                Assert.True(email.Successful);
                mockProvider.Verify(p => p.SendAsync(It.IsAny<IEmail>()), Times.Once);
            }
            finally
            {
                // Cleanup
                FluentCommsConfiguration.DefaultEmailProvider = null;
            }
        }

        [Fact]
        public async Task Sms_ExactSyntaxFromProblemStatement_Works()
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
                // Act - This is the exact syntax from the problem statement
                var email = await global::FluentComms.FluentComms.Sms()
                    .From("+15555554444")
                    .To("+15555552222")
                    .Body("yo bob, long time no see!")
                    .SendAsync();

                // Assert
                Assert.True(email.Successful);
                mockProvider.Verify(p => p.SendAsync(It.IsAny<ISms>()), Times.Once);
            }
            finally
            {
                // Cleanup
                FluentCommsConfiguration.DefaultSmsProvider = null;
            }
        }
    }
}