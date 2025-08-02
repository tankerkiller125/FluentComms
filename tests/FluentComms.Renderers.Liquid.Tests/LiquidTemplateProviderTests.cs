using FluentComms.Renderers.Liquid;

namespace FluentComms.Renderers.Liquid.Tests;

public class LiquidTemplateProviderTests
{
    private readonly LiquidTemplateProvider _provider;

    public LiquidTemplateProviderTests()
    {
        _provider = new LiquidTemplateProvider();
    }

    [Fact]
    public async Task RenderAsync_WithSimpleTemplate_ReturnsRenderedContent()
    {
        // Arrange
        var template = "Hello {{name}}!";
        var model = new { name = "World" };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public async Task RenderAsync_WithComplexModel_ReturnsRenderedContent()
    {
        // Arrange
        var template = "Dear {{user.first_name}} {{user.last_name}}, your order #{{order.id}} has been {{order.status}}.";
        var model = new
        {
            user = new { first_name = "John", last_name = "Doe" },
            order = new { id = 12345, status = "shipped" }
        };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("Dear John Doe, your order #12345 has been shipped.", result);
    }

    [Fact]
    public async Task RenderAsync_WithArrayIteration_ReturnsRenderedContent()
    {
        // Arrange
        var template = "Items: {% for item in items %}{{item.name}} - ${{item.price}}{% unless forloop.last %}, {% endunless %}{% endfor %}";
        var model = new
        {
            items = new[]
            {
                new { name = "Apple", price = 1.50 },
                new { name = "Orange", price = 2.00 },
                new { name = "Banana", price = 1.25 }
            }
        };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("Items: Apple - $1.5, Orange - $2, Banana - $1.25", result);
    }

    [Fact]
    public async Task RenderAsync_WithConditionals_ReturnsRenderedContent()
    {
        // Arrange
        var template = "{% if user.is_premium %}Welcome Premium User {{user.name}}!{% else %}Welcome {{user.name}}!{% endif %}";
        var premiumModel = new { user = new { name = "Alice", is_premium = true } };
        var regularModel = new { user = new { name = "Bob", is_premium = false } };

        // Act
        var premiumResult = await _provider.RenderAsync(template, premiumModel);
        var regularResult = await _provider.RenderAsync(template, regularModel);

        // Assert
        Assert.Equal("Welcome Premium User Alice!", premiumResult);
        Assert.Equal("Welcome Bob!", regularResult);
    }

    [Fact]
    public async Task RenderAsync_WithEmptyTemplate_ReturnsEmptyString()
    {
        // Arrange
        var template = "";
        var model = new { };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public async Task RenderAsync_WithPlainText_ReturnsPlainText()
    {
        // Arrange
        var template = "This is plain text without any liquid syntax.";
        var model = new { };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("This is plain text without any liquid syntax.", result);
    }

    [Fact]
    public async Task RenderAsync_WithInvalidSyntax_ThrowsInvalidOperationException()
    {
        // Arrange
        var template = "{{unclosed tag";
        var model = new { };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _provider.RenderAsync(template, model));
        Assert.Contains("Failed to parse Liquid template", exception.Message);
    }

    [Fact]
    public async Task RenderAsync_GenericMethod_ReturnsRenderedContent()
    {
        // Arrange
        var template = "Hello {{name}}!";
        var model = new TestModel { Name = "Generic" };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("Hello Generic!", result);
    }

    [Fact]
    public async Task RenderAsync_WithFilters_ReturnsFilteredContent()
    {
        // Arrange
        var template = "{{message | upcase}}";
        var model = new { message = "hello world" };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("HELLO WORLD", result);
    }

    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
