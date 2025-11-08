# BeSpoked Bikes - Sales Tracking API

A .NET 8 Web API for tracking bicycle sales, calculating commissions, and generating quarterly reports for BeSpoked Bikes.

## Project Overview

BeSpoked Bikes is a high-end bicycle shop that needs a backend system to track sales, manage inventory, and calculate salesperson commissions. This API provides the core functionality for managing customers, products, salespeople, and sales transactions.

## Architecture

This project follows **Clean Architecture** principles with a **feature-oriented folder structure**:

### Layers

- **BespokedBikes.Domain** - Domain entities and business logic
- **BespokedBikes.Application** - Application services, DTOs, validators, and mapping profiles
- **BespokedBikes.Infrastructure** - Data access, repositories, and EF Core DbContext
- **BespokedBikes.Api** - API controllers, middleware, and configuration
- **BespokedBikes.Tests.Unit** - Unit tests
- **BespokedBikes.Tests.Integration** - Integration tests

### Feature-Oriented Structure

Each feature is organized in its own folder containing all related files:

```
Features/
├── Customers/
│   ├── CustomerDto.cs
│   ├── CustomerService.cs
│   ├── ICustomerService.cs
│   ├── CustomerValidator.cs
│   ├── CustomerMappingProfile.cs
│   └── CustomerRepository.cs (in Infrastructure)
├── Products/
├── Salespeople/
├── Sales/
└── Reports/
```

## Technology Stack

- **.NET 8** - Web API framework
- **Entity Framework Core 8.x** - ORM for database access
- **PostgreSQL** - Database (via Npgsql.EntityFrameworkCore.PostgreSQL)
- **AutoMapper 12.0.1** - Object-to-object mapping
- **FluentValidation 11.10.0** - Request validation
- **Microsoft.Extensions.Logging** - Built-in logging with console output
- **OpenAPI / Scalar** - API documentation (instead of Swagger)
- **NUnit** - Testing framework

## Project Structure

```
bespoked-bikes/
├── BespokedBikes.Domain/
│   └── Entities/
│       ├── Customer.cs
│       ├── Employee.cs
│       ├── Salesperson.cs
│       ├── Product.cs
│       ├── Inventory.cs
│       └── Sale.cs
├── BespokedBikes.Application/
│   └── Features/
│       ├── Customers/
│       ├── Products/
│       ├── Salespeople/
│       ├── Sales/
│       └── Reports/
├── BespokedBikes.Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   └── Features/
│       ├── Customers/
│       ├── Products/
│       ├── Salespeople/
│       └── Sales/
├── BespokedBikes.Api/
│   ├── Features/
│   ├── Middleware/
│   ├── Program.cs
│   └── appsettings.json
├── BespokedBikes.Tests.Unit/
├── BespokedBikes.Tests.Integration/
├── openapi.yaml
└── BespokedBikes.sln
```

## Domain Model

### Entities

- **Customer** - Customer information (name, email, contact details, address)
- **Employee** - Base employee information
- **Salesperson** - Sales staff (references Employee, tracks start/termination dates)
- **Product** - Bicycle products (type, name, manufacturer, pricing, commission percentage)
- **Inventory** - Product inventory tracking by location
- **Sale** - Sales transactions (links customer, salesperson, product; tracks sale price and commission)

### Key Business Logic

- **Commission Calculation**: `CommissionAmount = (SalePrice - CostPrice) × CommissionPercentage`
- Commission is stored on each Sale for historical accuracy
- SalePrice can differ from RetailPrice (discounts, negotiations)
- Inventory is reduced when sales are created

## API Endpoints

The API is defined in `openapi.yaml` and includes:

### Customers

- `POST /api/customers` - Create customer
- `GET /api/customers/{id}` - Get customer by ID
- `PUT /api/customers/{id}` - Update customer
- `GET /api/customers?searchTerm=...` - Search customers

### Salespeople

- `POST /api/salespeople` - Create salesperson
- `GET /api/salespeople/{id}` - Get salesperson by ID
- `PUT /api/salespeople/{id}` - Update salesperson
- `GET /api/salespeople` - List salespeople

### Products

- `POST /api/products` - Create product
- `GET /api/products/{id}` - Get product by ID
- `PUT /api/products/{id}` - Update product
- `GET /api/products` - List products
- `PUT /api/products/{id}/inventory` - Update inventory

### Sales

- `POST /api/sales` - Create sale (calculates commission automatically)
- `GET /api/sales/{id}` - Get sale by ID
- `GET /api/sales?startDate=X&endDate=Y` - Filter sales by date range

### Reports

- `GET /api/reports/commissions/quarterly?year=2025&quarter=4` - Quarterly commission report
- `GET /api/reports/commissions/salesperson/{id}/quarterly?year=2025&quarter=4` - Salesperson quarterly report

## Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL 14+
- IDE (Visual Studio, VS Code, or Rider)

### Build the Solution

```bash
dotnet restore
dotnet build
```

### Run the API

```bash
cd BespokedBikes.Api
dotnet run
```

The API will start on `https://localhost:5001` (or configured port).

### Run Tests

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test BespokedBikes.Tests.Unit

# Run integration tests only
dotnet test BespokedBikes.Tests.Integration
```

## Configuration

Configuration is managed through `appsettings.json` and `appsettings.Development.json`.

### Connection String

Update the PostgreSQL connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=bespoked_bikes;Username=postgres;Password=postgres"
  }
}
```

### Logging

Microsoft.Extensions.Logging is configured with console and debug output providers for logging.

## Design Decisions

### Commission Storage on Sale

Commission percentage is stored on the Product, but the calculated commission amount is stored on each Sale. This preserves historical accuracy when commission rates change over time.

### SalePrice vs RetailPrice

Products have a RetailPrice, but each Sale has a SalePrice. This allows for discounts, promotions, and price negotiations without affecting the base product pricing.

### Feature-Oriented Folders

Instead of organizing by technical layers (Controllers, Services, DTOs in separate folders), we organize by business features. Each feature folder contains all related files, making it easier to understand and modify a specific feature.

### PostgreSQL Over SQLite

PostgreSQL provides better support for production workloads, concurrent access, and advanced features needed for a real-world sales tracking system.

### OpenAPI and Scalar Over Swagger

OpenAPI specification defines the API contract, and Scalar provides a modern, beautiful documentation UI. Controllers will be generated from the OpenAPI spec.

## Next Steps

- [ ] Implement entity classes with full properties
- [ ] Implement DTOs with full properties
- [ ] Generate controllers from OpenAPI spec (using code generator)
- [ ] Implement service business logic
- [ ] Implement repository data access
- [ ] Add EF Core migrations
- [ ] Add seed data for demonstration
- [ ] Implement FluentValidation rules
- [ ] Configure AutoMapper profiles
- [ ] Add middleware for global exception handling
- [ ] Docker containerization
- [ ] CI/CD pipeline setup

## Documentation

- [Class Diagram](docs/1%20initial%20diagrams/class-diagram.md)
- [Entity Relationship Diagram](docs/1%20initial%20diagrams/entity-relationship-diagram.md)
- [User Scenario Diagram](docs/1%20initial%20diagrams/user-scenario-diagram.md)
- [OpenAPI Specification](openapi.yaml)

## Commit Convention

This project uses [Conventional Commits](https://www.conventionalcommits.org/) for commit messages.

## planning phase

- [x] scope of deliverables
- [x] tech stack decision
- [x] architecture design
  - [x] initial sketches
  - [x] data model
  - [x] user stories
  - [x] api design
- [x] project plan
- [ ] setup project structure / monorepo
  - [ ] initial test projects with single passing test for each
- [ ] containerization with docker
- [ ] setup ci/cd pipeline
- [ ] start development

## AI tooling

I will be using AI tooling as an accelerator tool. for planning i wil use it to generate the actual diagram files form my sketches and then use it to iterate on those until they are the way I want them. during development I will use it to help me with boilerplate code generation and to help me think through problems or issues I encounter. i _will not_ use it to generate large chunks of code or features without my direct involvement. iw ill do my best to identify areas that not my own if there are any and indicate which decisions were made with AI assistance.

## commit history

i will use the commit history to "tell a story" more than anything since this is an interview exercise.

## decisions

- nswag to go from openapi spec to controllers quickly. make sure to generate dto types even thoguht hey may "not be needed" yet becasue its annoying to configure it without them
- monorepo structure to have a silly simple UI if we have time. apps/api, apps/web
- fluentmigrator for database migrations, i like their simplicity for this project
- entity framework core with code first approach for data layer. its quick to get going and easy to seed data for demo purposes
- sqlite for database, easy to setup and demo with minimal fuss
- dotnet 9
- react with jsx for frontend, minimal setup with vite
- refit and refitter for integration tests of the api without a frontend
- nunit for tests, fluentassertions
- docker for containerization
- github actions for ci/cd
- scalar instead of swagger... its prettier
- automapper for mapping between entities and dtos
- jest for unit tests on frontend
- "happy path" system tests with playwright if we have time
- exception middleware for error handling
- repository pattern for data access abstraction
- dependency injection for service management
- logging with microsoft.extensions.logging, json file logging provider
- instrumentation with opentelemetry, console exporter for now
- authorization that utilizes employee role for access
- identityserver for auth if we have time

## design docs

initial entity/class diagrams, they may evolve as we go.
[class-diagram.md](docs/1%20initial%20diagrams/class-diagram.md)
[entity-relationship-diagram.md](docs/1%20initial%20diagrams/entity-relationship-diagram.md)

the scenario doc/s were created to use as a starting point and an exercise to validate the initial entity/class designs. They may not reflect the final design decisions.
[user-scenario-diagram.md](docs/1%20initial%20diagrams/user-scenario-diagram.md)
