# bespoked-bikes

Demo project for bespoked-bikes.

## Commit Message Convention

This project uses [Conventional Commits](https://www.conventionalcommits.org/) for commit messages.

## planning phase

- [ ] scope of deliverables
- [ ] tech stack decision
- [ ] architecture design
  - [ ] initial sketches
  - [ ] data model
  - [ ] user stories
  - [ ] api design
- [ ] project plan

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
