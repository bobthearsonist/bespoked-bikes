# bespoked-bikes

Demo project for bespoked-bikes.

## Commit Message Convention

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
