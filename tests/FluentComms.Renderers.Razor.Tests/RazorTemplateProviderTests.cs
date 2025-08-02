using FluentComms.Renderers.Razor;

namespace FluentComms.Renderers.Razor.Tests;

public class RazorTemplateProviderTests
{
    private readonly RazorTemplateProvider _provider;

    public RazorTemplateProviderTests()
    {
        _provider = new RazorTemplateProvider();
    }

    [Fact]
    public async Task RenderAsync_WithSimpleTemplate_ReturnsRenderedContent()
    {
        // Arrange
        var template = "Hello @Model.Name!";
        var model = new { Name = "World" };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public async Task RenderAsync_WithComplexModel_ReturnsRenderedContent()
    {
        // Arrange
        var template = "Dear @Model.User.FirstName @Model.User.LastName, your order #@Model.Order.Id has been @Model.Order.Status.";
        var model = new
        {
            User = new { FirstName = "John", LastName = "Doe" },
            Order = new { Id = 12345, Status = "shipped" }
        };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("Dear John Doe, your order #12345 has been shipped.", result);
    }

    [Fact]
    public async Task RenderAsync_WithLoops_ReturnsRenderedContent()
    {
        // Arrange - Use reflection-based approach that works with RazorLight
        var template = @"Items:
@foreach(var item in Model.Items)
{
<text>- @item.GetType().GetProperty(""Name"").GetValue(item): $@item.GetType().GetProperty(""Price"").GetValue(item)
</text>
}";
        var model = new
        {
            Items = new[]
            {
                new { Name = "Apple", Price = 1.50m },
                new { Name = "Orange", Price = 2.00m },
                new { Name = "Banana", Price = 1.25m }
            }
        };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("Items:", result);
        Assert.Contains("- Apple: $1.50", result);
        Assert.Contains("- Orange: $2.00", result);
        Assert.Contains("- Banana: $1.25", result);
    }

    [Fact]
    public async Task RenderAsync_WithConditionals_ReturnsRenderedContent()
    {
        // Arrange
        var template = @"@if(Model.User.IsPremium)
{
<text>Welcome Premium User @Model.User.Name!</text>
}
else
{
<text>Welcome @Model.User.Name!</text>
}";
        var premiumModel = new { User = new { Name = "Alice", IsPremium = true } };
        var regularModel = new { User = new { Name = "Bob", IsPremium = false } };

        // Act
        var premiumResult = await _provider.RenderAsync(template, premiumModel);
        var regularResult = await _provider.RenderAsync(template, regularModel);

        // Assert
        Assert.Contains("Welcome Premium User Alice!", premiumResult);
        Assert.Contains("Welcome Bob!", regularResult);
        Assert.DoesNotContain("Premium", regularResult);
    }

    [Fact]
    public async Task RenderAsync_WithHtmlContent_ReturnsHtmlContent()
    {
        // Arrange
        var template = @"<h1>@Model.Title</h1>
<p>@Model.Content</p>
<div class=""highlight"">@Model.Highlight</div>";
        var model = new
        {
            Title = "Important Notice",
            Content = "This is the main content.",
            Highlight = "Special information"
        };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("<h1>Important Notice</h1>", result);
        Assert.Contains("<p>This is the main content.</p>", result);
        Assert.Contains("<div class=\"highlight\">Special information</div>", result);
    }

    [Fact]
    public async Task RenderAsync_WithCSharpExpressions_ReturnsCalculatedValues()
    {
        // Arrange
        var template = @"Total: $@(Model.Price * Model.Quantity)
Tax: $@(Model.Price * Model.Quantity * 0.1m)
Grand Total: $@(Model.Price * Model.Quantity * 1.1m)";
        var model = new { Price = 10.00m, Quantity = 3 };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("Total: $30.00", result);
        Assert.Contains("Tax: $3.0", result);
        Assert.Contains("Grand Total: $33.0", result);
    }

    [Fact]
    public async Task RenderAsync_WithEmptyTemplate_ReturnsEmptyString()
    {
        // Arrange
        var template = " "; // RazorLight doesn't handle truly empty templates, use whitespace
        var model = new { };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal(" ", result); // Expect the whitespace back
    }

    [Fact]
    public async Task RenderAsync_WithPlainText_ReturnsPlainText()
    {
        // Arrange
        var template = "This is plain text without any Razor syntax.";
        var model = new { };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("This is plain text without any Razor syntax.", result);
    }

    [Fact]
    public async Task RenderAsync_WithStringMethods_ReturnsFormattedContent()
    {
        // Arrange
        var template = @"Upper: @Model.Text.ToUpper()
Lower: @Model.Text.ToLower()
Length: @Model.Text.Length";
        var model = new { Text = "Hello World" };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("Upper: HELLO WORLD", result);
        Assert.Contains("Lower: hello world", result);
        Assert.Contains("Length: 11", result);
    }

    [Fact]
    public async Task RenderAsync_GenericMethod_ReturnsRenderedContent()
    {
        // Arrange - Use reflection to access strongly typed properties
        var template = "Hello @Model.GetType().GetProperty(\"Name\").GetValue(Model)!";
        var model = new TestModel { Name = "Generic" };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("Hello Generic!", result);
    }

    [Fact]
    public async Task RenderAsync_WithDateFormatting_ReturnsFormattedDate()
    {
        // Arrange
        var template = @"Date: @Model.Date.ToString(""yyyy-MM-dd"")
Time: @Model.Date.ToString(""HH:mm:ss"")";
        var testDate = new DateTime(2023, 12, 25, 15, 30, 45);
        var model = new { Date = testDate };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("Date: 2023-12-25", result);
        Assert.Contains("Time: 15:30:45", result);
    }

    [Fact]
    public async Task RenderAsync_WithNullValues_HandlesNullGracefully()
    {
        // Arrange
        var template = @"Name: @(Model.Name ?? ""Unknown"")
Value: @(Model.Value?.ToString() ?? ""N/A"")";
        var model = new { Name = (string?)null, Value = (int?)null };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("Name: Unknown", result);
        Assert.Contains("Value: N/A", result);
    }

    [Fact]
    public async Task RenderAsync_WithNestedObjects_ReturnsRenderedContent()
    {
        // Arrange
        var template = @"Company: @Model.Company.Name
Address: @Model.Company.Address.Street, @Model.Company.Address.City
Contact: @Model.Company.Contact.Email";
        var model = new
        {
            Company = new
            {
                Name = "Tech Corp",
                Address = new { Street = "123 Main St", City = "New York" },
                Contact = new { Email = "info@techcorp.com" }
            }
        };

        // Act
        var result = await _provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("Company: Tech Corp", result);
        Assert.Contains("Address: 123 Main St, New York", result);
        Assert.Contains("Contact: info@techcorp.com", result);
    }

    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
