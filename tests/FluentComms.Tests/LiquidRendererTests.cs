using Xunit;
using FluentComms.Renderers.Liquid;
using System.Threading.Tasks;

namespace FluentComms.Tests
{
    public class LiquidRendererTests
    {
        [Fact]
        public async Task ParseAsync_ShouldRenderLiquidTemplate_Correctly()
        {
            // Arrange
            var renderer = new LiquidRenderer();
            var template = "Hello, {{ name }}!";
            var model = new { name = "World" };

            // Act
            var result = await renderer.ParseAsync(template, model, false);

            // Assert
            Assert.Equal("Hello, World!", result);
        }

        [Fact]
        public async Task ParseAsync_ShouldThrowTemplateParseException_ForInvalidTemplate()
        {
            // Arrange
            var renderer = new LiquidRenderer();
            var template = "Hello, {{ name }"; // Missing closing brackets
            var model = new { name = "World" };

            // Act & Assert
            await Assert.ThrowsAsync<TemplateParseException>(() => renderer.ParseAsync(template, model, false));
        }
    }
}
