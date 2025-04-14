using Bunit;
using CarBlazor.Components.Datagrid;
using CarBlazor.Models;
using Microsoft.AspNetCore.Components;

namespace CarBlazorTest.BlazorTests;

public class DatagridTests : IDisposable
{
    private readonly TestContext _context;
    private readonly List<Car> _testCars;
    private readonly List<DataGridColumn<Car>> _carColumns;

    public DatagridTests()
    {
        _context = new TestContext();

        // Test data
        _testCars =
        [
            new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, Color = "Blue" },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2019, Color = "Red" },
            new Car { Id = 3, Make = "Ford", Model = "Mustang", Year = 2021, Color = "Black" }
        ];

        // Column definitions
        _carColumns =
        [
            new DataGridColumn<Car> { Title = "Make", ValueGetter = car => car.Make },
            new DataGridColumn<Car> { Title = "Model", ValueGetter = car => car.Model },
            new DataGridColumn<Car> { Title = "Year", ValueGetter = car => car.Year.ToString() },
            new DataGridColumn<Car> { Title = "Color", ValueGetter = car => car.Color }
        ];
    }

    [Fact]
    public void Datagrid_ShouldRender_WithCorrectColumnHeaders()
    {
        // Arrange
        var cut = _context.RenderComponent<Datagrid<Car>>(parameters => parameters
            .Add(p => p.Items, _testCars)
            .Add(p => p.Columns, _carColumns)
        );

        // Assert
        foreach (var column in _carColumns)
        {
            Assert.Contains(column.Title, cut.Markup);
        }
    }

    [Fact]
    public void Datagrid_ShouldRender_AllItems()
    {
        // Arrange
        var cut = _context.RenderComponent<Datagrid<Car>>(parameters => parameters
            .Add(p => p.Items, _testCars)
            .Add(p => p.Columns, _carColumns)
        );

        // Assert
        var rows = cut.FindAll("tbody tr");
        Assert.Equal(_testCars.Count, rows.Count);
    }

    [Fact]
    public void Datagrid_ShouldInvoke_OnRowClickCallback()
    {
        // Arrange
        Car? clickedCar = null;
        void HandleRowClick(Car? car) => clickedCar = car;

        var cut = _context.RenderComponent<Datagrid<Car>>(parameters => parameters
            .Add(p => p.Items, _testCars)
            .Add(p => p.Columns, _carColumns)
            .Add(p => p.OnRowClick, (Action<Car>)HandleRowClick)
        );

        // Act
        var firstRow = cut.Find("tbody tr");
        firstRow.Click();

        // Assert
        Assert.NotNull(clickedCar);
        Assert.Equal(_testCars[0].Id, clickedCar.Id);
    }

    [Fact]
    public void Datagrid_ShouldRender_RowActions()
    {
        // Arrange
        var deleteButtonClicked = false;

        var cut = _context.RenderComponent<Datagrid<Car>>(parameters => parameters
            .Add(p => p.Items, _testCars)
            .Add(p => p.Columns, _carColumns)
            .Add(p => p.RowActions, car => builder =>
            {
                builder.OpenElement(0, "button");
                builder.AddAttribute(1, "class", "delete-btn");
                builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(car, () => deleteButtonClicked = true));
                builder.AddContent(3, "Delete");
                builder.CloseElement();
            })
        );

        // Assert
        var deleteButtons = cut.FindAll(".delete-btn");
        Assert.Equal(_testCars.Count, deleteButtons.Count);

        // Act
        deleteButtons[0].Click();

        // Assert
        Assert.True(deleteButtonClicked);
    }

    [Fact]
    public void Datagrid_ShouldRender_EmptyMessage_WhenNoItems()
    {
        // Arrange
        var cut = _context.RenderComponent<Datagrid<Car>>(parameters => parameters
            .Add(p => p.Items, [])
            .Add(p => p.Columns, _carColumns)
            .Add(p => p.EmptyTemplate, "No cars found")
        );

        // Assert
        Assert.Contains("No cars found", cut.Markup);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}