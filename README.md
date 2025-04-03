# Car Blazor
A POC Blazor web application for managing car dealership operations including inventory, customer relationships, loans, and warranties.

## Features
- **Customer Management**: Manage customer information, including contact details and purchase history.
- **Inventory Management**: Track and manage car inventory, including details like make, model, year, and price.
- **Loan Management**: Manage customer loans, including application processing and payment tracking.
- **Warranty Management**: Keep track of warranties for cars, including coverage details and expiration dates.

## Technologies Used
- **Blazor**: A web framework for building interactive web applications using C# and .NET.
- **Entity Framework Core**: An ORM for .NET that enables database operations using C# objects.
- **SQLite**: A lightweight database engine for local data storage.
- **Bootstrap**: A CSS framework for responsive design and styling.

## Getting Started
### Prerequisites
- .NET 8.0 SDK or later

### Installation
1. Clone the repository
   ```
   git clone https://github.com/bsmylie873/car-blazor.git
   ``` 
2. Navigate to the project directory
   ```bash
   cd car-blazor
   ```
3. Restore the dependencies:
   ```bash
   dotnet restore
   ```
4. Apply the migrations:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
    dotnet run
    ```

## Usage
After launching the application, navigate to the homepage where you can manage the car inventory, manage customers, manage loans and manage warranties.