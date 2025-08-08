using Xunit;
using Moq;
using FluentComms.Core;
using FluentComms.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FluentComms.Tests
{
    public class CompositeRendererTests
    {
        private readonly Mock<ILogger<CompositeRenderer>> _mockLogger;
        private readonly object _testModel;

        public CompositeRendererTests()
        {
            _mockLogger = new Mock<ILogger<CompositeRenderer>>();
            _testModel = new { Name = "Test" };
        }

        [Fact]
        public async Task ParseAsync_ShouldProcessTemplateThroughPipeline()
        {
            // Arrange
            var mockRenderer1 = new Mock<IRenderer>();
            var mockRenderer2 = new Mock<IRenderer>();

            var initialTemplate = "Hello {{ Name }}";
            var intermediateTemplate = "Hello Test";
            var finalTemplate = "<p>Hello Test</p>";

            mockRenderer1.Setup(r => r.ParseAsync(initialTemplate, _testModel, false))
                         .ReturnsAsync(intermediateTemplate);
            mockRenderer2.Setup(r => r.ParseAsync(intermediateTemplate, _testModel, false))
                         .ReturnsAsync(finalTemplate);

            var renderers = new[] { mockRenderer1.Object, mockRenderer2.Object };
            var compositeRenderer = new CompositeRenderer(renderers, _mockLogger.Object);

            // Act
            var result = await compositeRenderer.ParseAsync(initialTemplate, _testModel, false);

            // Assert
            Assert.Equal(finalTemplate, result);
            mockRenderer1.Verify(r => r.ParseAsync(initialTemplate, _testModel, false), Times.Once);
            mockRenderer2.Verify(r => r.ParseAsync(intermediateTemplate, _testModel, false), Times.Once);
        }

        [Fact]
        public async Task ParseAsync_ShouldReturnOriginalTemplate_WhenNoRenderersConfigured()
        {
            // Arrange
            var renderers = Enumerable.Empty<IRenderer>();
            var compositeRenderer = new CompositeRenderer(renderers, _mockLogger.Object);
            var template = "Some content";

            // Act
            var result = await compositeRenderer.ParseAsync(template, _testModel, false);

            // Assert
            Assert.Equal(template, result);
        }

        [Fact]
        public async Task ParseAsync_ShouldThrowException_WhenRendererFails()
        {
            // Arrange
            var mockRenderer = new Mock<IRenderer>();
            var exception = new InvalidOperationException("Renderer failed");

            mockRenderer.Setup(r => r.ParseAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()))
                        .ThrowsAsync(exception);

            var renderers = new[] { mockRenderer.Object };
            var compositeRenderer = new CompositeRenderer(renderers, _mockLogger.Object);

            // Act & Assert
            var thrownException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                compositeRenderer.ParseAsync("template", _testModel, false));
            Assert.Equal(exception.Message, thrownException.Message);
        }
    }
}
