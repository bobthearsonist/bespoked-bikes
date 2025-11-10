# bespoked-bikes

Demo project for bespoked-bikes.

## Links

- [OpenAPI Specification](openapi.yaml)
- [Github Repository](https://github.com/yourusername/bespoked-bikes)

## Getting Started

### Build the Solution

```bash
cd apps/backend
dotnet restore
dotnet build
```

### Run the API

The API is defined in `openapi.yaml`.

```bash
cd BespokedBikes.Api
dotnet run
```

The API will start on `https://localhost:7150` (or configured port).
The API can be viewed at `https://localhost:7150/scalar/v1` when running locally.

### Run Tests

```bash
# Run all tests
dotnet test

# individual suites
dotnet test BespokedBikes.Tests.Unit
dotnet test BespokedBikes.Tests.Integration

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test suite with coverage
dotnet test BespokedBikes.Tests.Unit --collect:"XPlat Code Coverage"
```

Coverage reports are generated in the `TestResults` directory as `coverage.cobertura.xml` files.

## CI/CD Pipeline

This project uses GitHub Actions for continuous integration and delivery.

### Pipeline Overview

The CI/CD pipeline runs on every pull request to `main` or `develop` branches and executes the following jobs in parallel:

- **Backend Unit Tests**: Runs unit tests for the backend with code coverage enabled
- **Backend Integration Tests**: Runs integration tests for the backend with code coverage enabled
- **Frontend Unit Tests**: Placeholder for frontend unit tests (when frontend is added)
- **Frontend Integration Tests**: Placeholder for frontend integration tests (when frontend is added)
- **System Tests**: Runs after all component tests complete (currently stubbed for container setup)

After the PR CI/CD Pipeline completes, a separate workflow runs to generate and post coverage reports:

- **Coverage Report**: Downloads coverage artifacts from test runs and posts a summary comment on the PR

### Workflow Files

- **`.github/workflows/pr-ci.yml`**: Main PR workflow that orchestrates all test jobs
- **`.github/workflows/backend-test.yml`**: Reusable workflow for backend testing (reduces duplication)
- **`.github/workflows/coverage-report.yml`**: Workflow that runs after PR CI/CD to generate and post coverage reports

### Architecture

The pipeline uses reusable workflows to avoid code duplication. The backend test workflow accepts a `test-type` parameter (unit or integration) and is called by the main PR workflow twice - once for each test type.

All test jobs run with code coverage enabled using coverlet. Coverage reports are uploaded as artifacts and then collected by the coverage-report workflow, which posts a summary comment on the PR with:
- Line and branch coverage percentages
- Coverage badges
- Detailed coverage breakdown by project

### Future Enhancements

- Frontend test jobs will be populated when the frontend is implemented (with coverage enabled)
- System tests will be expanded once Docker containers are set up
- Additional jobs can be added for linting and deployment

## Development

### Generate Code from OpenAPI Spec

This project uses NSwag to generate controllers and DTOs from the OpenAPI specification. Run this whenever you update `openapi.yaml`:

```bash
cd apps/backend
nswag run nswag.json
```

This generates:

- **Controllers**: `BespokedBikes.Api/Controllers/GeneratedControllerBase.cs`
- **DTOs**: `BespokedBikes.Application/Generated/Dtos.cs`

### Test API Endpoints with HTTP Client

The project includes an HTTP client file (`apps/backend/BespokedBikes.Api/BespokedBikes.Api.http`) containing pre-written HTTP requests for testing the API endpoints. This file uses environment variables defined in `apps/backend/BespokedBikes.Api/http-client.env.json`.

To use it:

1. Ensure the API is running (see "Run the API" section)
2. Open `BespokedBikes.Api.http` in your IDE or text editor
3. The file supports variables like `{{BespokedBikes.Api_HostAddress}}` which are resolved from `http-client.env.json`
4. Execute requests directly from your IDE (most popular IDEs support HTTP file execution)

This provides a quick way to test API functionality without building a full frontend or using external tools like Postman.

### Commit Convention

This project uses [Conventional Commits](https://www.conventionalcommits.org/) for commit messages.

## Architecture

This backend project follows **Clean Architecture** with a feature-oriented organization. This is a quick jumble of words trying to describe _why_ its this "complex". I'm usually bad about remembering these terms when discussing the _why_ so now I cant forget.

```text
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

- **.NET 9** - Web API framework
- **Entity Framework Core 9.0** - ORM for database access
- **PostgreSQL** - Database (via Npgsql.EntityFrameworkCore.PostgreSQL 9.0.2)
- **AutoMapper 12.0.1** - Object-to-object mapping
- **FluentValidation 11.10.0** - Request validation
- **Microsoft.Extensions.Logging** - Built-in logging with console output
- **OpenAPI / Scalar** - API documentation (instead of Swagger)
- **NUnit** - Testing framework
- **FluentAssertions** - Assertion library for tests

## Domain Model

### Entities

- **Customer**
- **Employee**
- **Product**
- **Sale**
- **Inventory**

## Design Decisions and Planning

### Design Decisions

#### Commission Storage on Sale

Commission percentage is stored on the Product, but the calculated commission amount is stored on each Sale. This preserves historical accuracy when commission rates change over time.

- **Commission Calculation**: `CommissionAmount = (SalePrice - CostPrice) × CommissionPercentage`
- Commission is stored on each Sale for historical accuracy
- SalePrice can differ from RetailPrice (discounts, negotiations)
- Inventory is reduced when sales are created

#### SalePrice vs RetailPrice

Products have a RetailPrice, but each Sale has a SalePrice. This allows for discounts, promotions, and price negotiations without affecting the base product pricing.

#### Feature-Oriented Folders

Instead of organizing by technical layers (Controllers, Services, DTOs in separate folders), we organize by business features. Each feature folder contains all related files, making it easier to understand and modify a specific feature.

(I let this fall away a bit in some of the other layers because it kind of made more sense in some places than others...)

#### OpenAPI and Scalar Over Swagger

OpenAPI specification defines the API contract, and Scalar provides a modern, beautiful documentation UI. Controllers will be generated from the OpenAPI spec.

#### AutoMapper with Attributes

We're using attributes which keeps boilerplate as simple as possible. There are some places this isn't working currently that should be resolved when we clean up the DTO form the service layer and the data layer schema leaking upward.

#### Controller Layout and nswag

I don't like the current controller layout, I would like to have individual controller files with the routes defined there. I was wrestling with nswag a little too much getting that running and moved on since it's really just organizational.

* better layout in scalar per controller
* better folder organization
* possibly more annoying DI setup

#### DateTimeOffset

We're using DateTimeOffset instead of DateTime to handle date and time values. This ensures that we have a consistent representation of time across different time zones and avoids issues with daylight saving time.

I discovered some of the way through that I had picked DateTime for the formats and should likely be using DateTimeOffset. I found it before we got to the Sale objects which will have some time values in the API contract. It just makes it a lot easier to work across time zones with practically zero overheads.

### planning phase

- [x] scope of deliverables
- [x] tech stack decision
- [x] architecture design
  - [x] initial sketches
  - [x] data model
  - [x] user stories
  - [x] api design
- [x] project plan
- [x] setup project structure / monorepo
  - [x] initial test projects with single passing test for each
- [ ] containerization with docker
- [ ] setup ci/cd pipeline
- [x] start development

### Next Steps

- [x] Implement entity classes with full properties
- [x] Implement DTOs with full properties
- [x] Generate controllers from OpenAPI spec (using code generator)
- [x] Implement service business logic
- [x] Implement repository data access
- [x] ~~Add EF Core migrations~~ Add fluentmigrator migrations
- [x] Add seed data for demonstration
- [ ] Implement FluentValidation rules
- [x] Configure AutoMapper profiles
- [ ] Add middleware for global exception handling
- [ ] Docker containerization
- [ ] CI/CD pipeline setup

### punts

- [ ] we shouldnt be using the dto in the service layer. we should be doing that mapping in the api layer. when you go back to fix this make sure to wind up with something using more annotations and less strict mapping classes, not less
- [ ] we need to get to a real db asap
- [ ] the middleware needs to be setup, your exposing stack traces on errors RN
- [ ] TESTS!!!
- [ ] ODATA for querying would be a quick win to add filtering/paging/sorting
- [ ] Inventory update is kind of ugly. maybe use a JSON PATCH model.
- [ ] we need to finish cleaning up the createddate/modifieddate handling adn remove it form the dto's
- [ ] im not sold on the infastructure/domain/application layering. its making it really hard to review changes wholistically. the separation of concerns is great, but for speedrunning through this exercise its slowing me down and making me miss things in review.

### decisions

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
- ODATA for querying if we have time

### design docs

initial entity/class diagrams, they may evolve as we go.
[class-diagram.md](docs/1%20initial%20diagrams/class-diagram.md)
[entity-relationship-diagram.md](docs/1%20initial%20diagrams/entity-relationship-diagram.md)

the scenario doc/s were created to use as a starting point and an exercise to validate the initial entity/class designs. They may not reflect the final design decisions.
[user-scenario-diagram.md](docs/1%20initial%20diagrams/user-scenario-diagram.md)

### AI tooling

I will be using AI tooling as an accelerator tool. for planning i wil use it to generate the actual diagram files form my sketches and then use it to iterate on those until they are the way I want them. during development I will use it to help me with boilerplate code generation and to help me think through problems or issues I encounter. i _will not_ use it to generate large chunks of code or features without my direct involvement. iw ill do my best to identify areas that not my own if there are any and indicate which decisions were made with AI assistance.

### commit history

i will use the commit history to "tell a story" more than anything since this is an interview exercise.
