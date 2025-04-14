using Bunit;
using CarBlazor.Components.Search;
using Microsoft.AspNetCore.Components.Web;

namespace CarBlazorTest.BlazorTests;

public class SearchComponentTests : TestContext
{
    [Fact]
    public void Search_RendersWithDefaultPlaceholder()
    {
        // Arrange & Act
        var cut = RenderComponent<Search>();
        
        // Assert
        var input = cut.Find("input");
        Assert.Equal("Search...", input.GetAttribute("placeholder"));
    }
    
    [Fact]
    public void Search_RendersWithCustomPlaceholder()
    {
        // Arrange & Act
        var cut = RenderComponent<Search>(parameters => 
            parameters.Add(p => p.Placeholder, "Find cars..."));
        
        // Assert
        var input = cut.Find("input");
        Assert.Equal("Find cars...", input.GetAttribute("placeholder"));
    }
    
    [Fact]
    public void Search_TriggersOnSearchEventWhenTyping()
    {
        // Arrange
        var lastSearchTerm = "Mercedes";
        var cut = RenderComponent<Search>(parameters =>
            parameters.Add(p => p.OnSearch, term => lastSearchTerm = term));

        // Act
        var input = cut.Find("input");
        input.Input("BMW");
        input.TriggerEvent("onkeyup", new KeyboardEventArgs());

        // Assert
        Assert.Equal("BMW", lastSearchTerm);
    }
    
    [Fact]
    public void Search_ClearButtonAppearsWhenTextIsEntered()
    {
        // Arrange
        var cut = RenderComponent<Search>();
        
        // Act
        var input = cut.Find("input");
        input.Input("test");
        
        // Assert
        var clearButton = cut.FindAll("button");
        Assert.Single(clearButton);
    }
    
    [Fact]
    public void Search_ClearButtonClearsTextAndTriggersEvent()
    {
        // Arrange
        string lastSearchTerm = null;
        var cut = RenderComponent<Search>(parameters => 
            parameters.Add(p => p.OnSearch, (string term) => lastSearchTerm = term));
        
        // Act - First enter text
        var input = cut.Find("input");
        input.Input("test");
        
        // Act - Then clear it
        var clearButton = cut.Find("button");
        clearButton.Click();
        
        // Assert
        Assert.Empty(input.GetAttribute("value") ?? string.Empty);
        Assert.Empty(lastSearchTerm);
    }
}