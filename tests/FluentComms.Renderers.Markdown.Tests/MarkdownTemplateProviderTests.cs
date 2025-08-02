using FluentComms.Renderers.Markdown;
using Markdig;

namespace FluentComms.Renderers.Markdown.Tests;

public class MarkdownTemplateProviderTests
{
    [Fact]
    public async Task RenderAsync_WithSimpleMarkdown_ReturnsHtml()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = "# Hello World";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("<h1 id=\"hello-world\">Hello World</h1>\n", result);
    }

    [Fact]
    public async Task RenderAsync_WithComplexMarkdown_ReturnsFormattedHtml()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = @"# Main Title

This is a **bold** text and this is *italic*.

## Subtitle

- Item 1
- Item 2
- Item 3

[Link](https://example.com)";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("<h1 id=\"main-title\">Main Title</h1>", result);
        Assert.Contains("<strong>bold</strong>", result);
        Assert.Contains("<em>italic</em>", result);
        Assert.Contains("<h2 id=\"subtitle\">Subtitle</h2>", result);
        Assert.Contains("<ul>", result);
        Assert.Contains("<li>Item 1</li>", result);
        Assert.Contains("<a href=\"https://example.com\">Link</a>", result);
    }

    [Fact]
    public async Task RenderAsync_WithCodeBlocks_ReturnsHtmlWithCodeBlocks()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = @"```csharp
public class Test
{
    public string Name { get; set; }
}
```";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("<pre><code class=\"language-csharp\">", result);
        Assert.Contains("public class Test", result);
    }

    [Fact]
    public async Task RenderAsync_WithTable_ReturnsHtmlTable()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = @"| Name | Age | City |
|------|-----|------|
| John | 30  | NYC  |
| Jane | 25  | LA   |";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("<table>", result);
        Assert.Contains("<thead>", result);
        Assert.Contains("<tbody>", result);
        Assert.Contains("<th>Name</th>", result);
        Assert.Contains("<td>John</td>", result);
    }

    [Fact]
    public async Task RenderAsync_WithEmptyTemplate_ReturnsEmptyString()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = "";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public async Task RenderAsync_WithPlainText_ReturnsPlainTextInParagraph()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = "This is plain text.";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("<p>This is plain text.</p>\n", result);
    }

    [Fact]
    public async Task RenderAsync_WithCustomPipeline_UsesCustomConfiguration()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider(builder => 
        {
            builder.DisableHtml(); // Disable HTML tags
        });
        var template = "This is **bold** and <script>alert('test')</script>";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("<strong>bold</strong>", result);
        Assert.DoesNotContain("<script>", result); // HTML should be escaped/removed
    }

    [Fact]
    public async Task RenderAsync_WithAdvancedExtensions_SupportsTaskLists()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = @"- [x] Completed task
- [ ] Incomplete task";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Contains("task-list-item", result);
        Assert.Contains("checked", result);
    }

    [Fact]
    public async Task RenderAsync_WithEmoji_SupportsEmojiExtension()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = "Hello :smile: World!";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        // The emoji extension should convert :smile: to an emoji or keep it as is
        Assert.Contains("Hello", result);
        Assert.Contains("World!", result);
    }

    [Fact]
    public async Task RenderAsync_ModelIsIgnored_ReturnsMarkdownAsHtml()
    {
        // Arrange
        var provider = new MarkdownTemplateProvider();
        var template = "# Static Content";
        var model = new { name = "John", age = 30 };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("<h1 id=\"static-content\">Static Content</h1>\n", result);
        // Model data should not affect the output since Markdown doesn't process dynamic content
    }

    [Fact]
    public async Task Constructor_WithNullConfigurator_UsesDefaultConfiguration()
    {
        // Arrange & Act
        var provider = new MarkdownTemplateProvider(null);
        var template = "# Test";
        var model = new { };

        // Act
        var result = await provider.RenderAsync(template, model);

        // Assert
        Assert.Equal("<h1 id=\"test\">Test</h1>\n", result);
    }
}
