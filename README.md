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
- **PostgreSQL**: A powerful, open-source relational database system.
- **Docker**: Containerization platform for running PostgreSQL.
- **Bootstrap**: A CSS framework for responsive design and styling.

## Getting Started
### Prerequisites
- .NET 8.0 SDK or later
- Docker and Docker Compose

### Installation
1. Clone the repository
   ```
   git clone https://github.com/bsmylie873/car-blazor.git
   ``` 
2. Navigate to the project directory
   ```bash
   cd car-blazor
   ```
3. Start the PostgreSQL database using Docker Compose
   ```bash
   docker-compose up -d
   ```
4. Restore the dependencies:
   ```bash
   dotnet restore
   ```
5. Apply the migrations:
   ```bash
   dotnet ef database update
   ```
6. Run the application:
   ```bash
    dotnet run
    ```

## Usage
After launching the application, navigate to the homepage where you can manage the car inventory, manage customers, manage loans and manage warranties.


# ERD
```mermaid
erDiagram
    cars {
        text color 
        integer id PK 
        text make 
        text model 
        timestamp_without_time_zone production_date 
        integer year 
    }

    customer_cars {
        integer car_id FK 
        integer customer_id FK 
        integer id PK 
        boolean is_primary_owner 
        timestamp_without_time_zone purchase_date 
    }

    customers {
        text address 
        text email 
        text first_name 
        integer id PK 
        text last_name 
        text phone_number 
    }

    flyway_schema_history {
        integer checksum 
        character_varying description 
        integer execution_time 
        character_varying installed_by 
        timestamp_without_time_zone installed_on 
        integer installed_rank PK 
        character_varying script 
        boolean success 
        character_varying type 
        character_varying version 
    }

    loan_statuses {
        integer id PK 
        text name 
    }

    loans {
        numeric amount 
        integer car_id FK 
        integer customer_id FK 
        timestamp_without_time_zone end_date 
        integer id PK 
        numeric interest_rate 
        numeric monthly_payment 
        numeric remaining_balance 
        timestamp_without_time_zone start_date 
        integer status_id FK 
    }

    roles {
        text description 
        text id PK 
        text name 
    }

    users {
        timestamp_without_time_zone created_at 
        boolean force_password_reset 
        integer id PK 
        text password_hash 
        text role_id FK 
        text salt 
        text username 
    }

    warranties {
        integer car_id FK 
        numeric cost 
        text coverage_details 
        timestamp_without_time_zone end_date 
        integer id PK 
        text provider 
        timestamp_without_time_zone start_date 
        integer warranty_type_id FK 
    }

    warranty_types {
        integer id PK 
        text name 
    }

    customer_cars }o--|| cars : "car_id"
    loans }o--|| cars : "car_id"
    warranties }o--|| cars : "car_id"
    customer_cars }o--|| customers : "customer_id"
    loans }o--|| customers : "customer_id"
    loans }o--|| loan_statuses : "status_id"
    users }o--|| roles : "role_id"
    warranties }o--|| warranty_types : "warranty_type_id"
```