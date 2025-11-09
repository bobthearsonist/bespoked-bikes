# bespoked-bikes

Demo project for bespoked-bikes.

This project uses [Conventional Commits](https://www.conventionalcommits.org/) for commit messages.


## Architecture

This backend project follows **Clean Architecture** with a feature-oriented organization. This is a quick jumble of words trying to describe *why* its this "complex". I'm usually bad about remembering these terms when discussing the *why* so now I cant forget.

``` text
Domain (entities, enums) → no dependencies
   ↑
Application (services, DTOs, interfaces) → depends on Domain
   ↑
Infrastructure (repositories, DbContext) → depends on Domain + Application
   ↑
Api (controllers, middleware) → depends on Application + Infrastructure
```

Achieves **SOLID** principles:

| Principle                                          | Implementation                                                 |
| -------------------------------------------------- | -------------------------------------------------------------- |
| **S** - Single Responsibility                      | Each project/class has one job                                 |
| **O** - Open for extension/Closed for Modification | Extend via interfaces/inheritance                              |
| **L** - Liskov Substitution                        | IEmployeeRepository substitutable with any implementation      |
| **I** - Interface Segregation                      | Small, focused interfaces (IEmployeeRepository, ISalesService) |
| **D** - Dependency Inversion                       | Application defines interfaces, Infrastructure implements them |

## Technology Stack

### Backend
- **.NET 9** - Web API framework
- **Entity Framework Core 9.0** - ORM for database access
- **PostgreSQL** - Database (via Npgsql.EntityFrameworkCore.PostgreSQL 9.0.2)
- **AutoMapper 12.0.1** - Object-to-object mapping
- **FluentValidation 11.10.0** - Request validation
- **Microsoft.Extensions.Logging** - Built-in logging with console output
- **OpenAPI / Scalar** - API documentation (instead of Swagger)
- **NUnit** - Testing framework
- **FluentAssertions** - Assertion library for tests

### Frontend
- **React 19** with JSX - UI framework
- **Vite 7** - Build tool and dev server
- **Jest 30** - Testing framework
- **React Testing Library 16** - Component testing utilities

## Domain Model

### Entities

- [ ] align this section with result of actual design

### Key Business Logic

- **Commission Calculation**: `CommissionAmount = (SalePrice - CostPrice) × CommissionPercentage`
- Commission is stored on each Sale for historical accuracy
- SalePrice can differ from RetailPrice (discounts, negotiations)
- Inventory is reduced when sales are created

## Getting Started

### Build the Solution

```bash
dotnet restore
dotnet build
```

### Run the API

The API is defined in `openapi.yaml`.

```bash
cd BespokedBikes.Api
dotnet run
```

The API will start on `https://localhost:5001` (or configured port).
The API can be viewed at `https://localhost:5001/scalar` when running locally.

### Run Tests

```bash
# Run all backend tests
dotnet test

# individual suites
dotnet test BespokedBikes.Tests.Unit
dotnet test BespokedBikes.Tests.Integration
```

### Run the Frontend

The React frontend is located in `apps/web` and provides a basic UI for creating sales.

```bash
cd apps/web
npm install
npm run dev
```

The frontend will start on `http://localhost:5173` (or another port if 5173 is in use).

**Note:** The backend API must be running for the frontend to function properly. See [apps/web/BACKEND_STATUS.md](apps/web/BACKEND_STATUS.md) for current backend implementation status.

```bash
# Run frontend tests
cd apps/web
npm test
```

For more details, see the [Frontend README](apps/web/README.md).

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
