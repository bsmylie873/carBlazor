using CarBlazor.Components.Datagrid;
using CarBlazor.DAL.Models;

namespace CarBlazor.Components.Constants;

public static class DatagridColumnDefinitions
{
    public static readonly List<DataGridColumn<Car>> CarColumns =
    [
        new()
        {
            Title = "Make",
            ValueGetter = car => car.Make
        },

        new()
        {
            Title = "Model",
            ValueGetter = car => car.Model
        },

        new()
        {
            Title = "Year",
            ValueGetter = car => car.Year
        },

        new()
        {
            Title = "Color",
            ValueGetter = car => car.Color
        },

        new()
        {
            Title = "Production Date",
            ValueGetter = car => car.ProductionDate?.ToShortDateString()
        }
    ];
    
    public static readonly List<DataGridColumn<Customer>> CustomerColumns =
    [
        new()
        {
            Title = "Full Name",
            ValueGetter = customer => customer.FullName
        },

        new()
        {
            Title = "Email",
            ValueGetter = customer => customer.Email
        },

        new()
        {
            Title = "Address",
            ValueGetter = customer => customer.Address
        },

        new()
        {
            Title = "Phone Number",
            ValueGetter = customer => customer.PhoneNumber
        }
    ];
    
    public static readonly List<DataGridColumn<Loan>> LoanColumns =
    [
        new()
        {
            Title = "Car",
            ValueGetter = loan => $"{loan.Car?.Make} {loan.Car?.Model} ({loan.Car?.Year})"
        },

        new()
        {
            Title = "Amount",
            ValueGetter = loan => loan.Amount.ToString("C")
        },

        new()
        {
            Title = "Interest Rate",
            ValueGetter = loan => loan.InterestRate.ToString("P2")
        },

        new()
        {
            Title = "Start Date",
            ValueGetter = loan => loan.StartDate.ToShortDateString()
        },
        
        new()
        {
            Title = "End Date",
            ValueGetter = loan => loan.EndDate.ToShortDateString()
        },
        
        new()
        {
            Title = "Status",
            ValueGetter = loan => loan.Status?.Name
        }
    ];
    
    public static readonly List<DataGridColumn<User>> UserColumns =
    [
        new()
        {
            Title = "Username",
            ValueGetter = user => user.Username
        },

        new()
        {
            Title = "Role",
            ValueGetter = user => user.Role.Name
        },

        new()
        {
            Title = "Created At",
            ValueGetter = user => user.CreatedAt.ToLocalTime().ToString("g")
        }
    ];
    
    public static readonly List<DataGridColumn<Warranty>> WarrantyColumns =
    [
        new()
        {
            Title = "Car",
            ValueGetter = warranty => $"{warranty.Car?.Make} {warranty.Car?.Model} ({warranty.Car?.Year})"
        },

        new()
        {
            Title = "Warranty Type",
            ValueGetter = warranty => warranty.WarrantyType?.Name
        },

        new()
        {
            Title = "Provider",
            ValueGetter = warranty => warranty.Provider
        },
        
        new()
        {
            Title = "Start Date",
            ValueGetter = warranty => warranty.StartDate.ToShortDateString()
        },
        
        new()
        {
            Title = "End Date",
            ValueGetter = warranty => warranty.EndDate.ToShortDateString()
        },
        
        new()
        {
            Title = "Cost",
            ValueGetter = warranty => warranty.Cost.ToString("C")
        }
    ];
}