using Bunit;
using Xunit;
using CarBlazor.Components.FormModal;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace CarBlazorTest.BlazorTests;

public class FormModalTests : TestContext
{
    // Test model class
    private class TestModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }
    }

    [Fact]
    public void FormModal_ShouldBeHiddenByDefault()
    {
        // Arrange
        var model = new TestModel();

        // Act
        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.Title, "Test Form")
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0, "Test Content")));

        // Assert
        var modalDiv = cut.Find(".modal");
        Assert.DoesNotContain("show", modalDiv.GetAttribute("class"));
        Assert.Equal("none", modalDiv.GetAttribute("style").Split(';')
            .FirstOrDefault(s => s.Contains("display"))?.Split(':')[1].Trim());
    }

    [Fact]
    public async Task Show_ShouldDisplayModal()
    {
        // Arrange
        var model = new TestModel();
        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0, "Test Content")));

        // Act
        await cut.InvokeAsync(() => cut.Instance.Show());

        // Assert
        var modalDiv = cut.Find(".modal");
        Assert.Contains("show", modalDiv.GetAttribute("class"));
        Assert.Equal("block", modalDiv.GetAttribute("style").Split(';')
            .FirstOrDefault(s => s.Contains("display"))?.Split(':')[1].Trim());
    }

    [Fact]
    public async Task Hide_ShouldHideModal()
    {
        // Arrange
        var model = new TestModel();
        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0, "Test Content")));

        await cut.InvokeAsync(() => cut.Instance.Show());

        // Act
        await cut.InvokeAsync(() => cut.Instance.Hide());

        // Assert
        var modalDiv = cut.Find(".modal");
        Assert.DoesNotContain("show", modalDiv.GetAttribute("class"));
    }

    [Fact]
    public async Task Title_ShouldDisplayCorrectly()
    {
        // Arrange
        var model = new TestModel();
        var title = "Custom Form Title";

        // Act
        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.Title, title)
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0, "Test Content")));

        await cut.InvokeAsync(() => cut.Instance.Show());

        // Assert
        var titleElement = cut.Find(".modal-title");
        Assert.Equal(title, titleElement.TextContent);
    }

    [Fact]
    public async Task SaveButtonText_ShouldDisplayCorrectly()
    {
        // Arrange
        var model = new TestModel();
        var buttonText = "Submit Form";

        // Act
        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.SaveButtonText, buttonText)
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0, "Test Content")));

        await cut.InvokeAsync(() => cut.Instance.Show());

        // Assert
        var submitButton = cut.Find("button[type='submit']");
        Assert.Equal(buttonText, submitButton.TextContent);
    }

    [Fact]
    public async Task HandleSave_ShouldTriggerOnSaveCallback()
    {
        // Arrange
        var model = new TestModel { Name = "Test User", Age = 30 };
        TestModel? savedModel = null;

        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0, "Test Content"))
            .Add(p => p.OnSave, EventCallback.Factory.Create<TestModel>(this, m => savedModel = m)));

        await cut.InvokeAsync(() => cut.Instance.Show());

        // Act
        cut.Find("form").Submit();

        // Assert
        Assert.NotNull(savedModel);
        Assert.Equal("Test User", savedModel?.Name);
        Assert.Equal(30, savedModel?.Age);
    }

    [Fact]
    public async Task HandleCancel_ShouldTriggerOnCancelCallback()
    {
        // Arrange
        var model = new TestModel();
        var cancelCalled = false;

        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0, "Test Content"))
            .Add(p => p.OnCancel, EventCallback.Factory.Create(this, () => cancelCalled = true)));

        await cut.InvokeAsync(() => cut.Instance.Show());

        // Act
        cut.Find("button.btn-secondary").Click();

        // Assert
        Assert.True(cancelCalled);
    }

    [Fact]
    public async Task ValidationError_ShouldPreventFormSubmission()
    {
        // Arrange
        var model = new TestModel { Name = "" };
        var saveCallCount = 0;

        var cut = RenderComponent<FormModal<TestModel>>(parameters => parameters
            .Add(p => p.Model, model)
            .Add(p => p.ChildContent, content => content.AddMarkupContent(0,
                "<div><input id=\"name\" @bind-value=\"context.Name\" /></div>"))
            .Add(p => p.OnSave, EventCallback.Factory.Create<TestModel>(this, _ => saveCallCount++)));

        await cut.InvokeAsync(() => cut.Instance.Show());

        // Act
        cut.Find("form").Submit();

        // Assert
        Assert.Equal(0, saveCallCount);
        Assert.Contains("validation", cut.Markup.ToLower());
    }
}