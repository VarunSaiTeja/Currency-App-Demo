# Currency

## Setup Instructions

1. **Update Database**
   ```sh
   update-database
   ```
2. **Run Jaeger (for tracing):**
   ```sh
   docker run --name jager -d -p 4317:4317 -p 16686:16686 --restart unless-stopped jaegertracing/all-in-one:latest
   ```
3. **Run Seq (for logging):**
   ```sh
   docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
   ```

## Assumptions Made
- Admin can manage roles of other users
- In future, can switch to any currency exchange rate provider

## Possible Future Enhancements
- Distributed caching
- Reverse proxy

## Architecture
- Vertical slices

## Patterns Used
- SOLID Principles
- Factory
- Strategy
- Mediator
- Repository
- UnitOfWork
- Specification
- Dependency Injection
- Options Pattern
- Keyed Services

## Folder and Project Structure
- **Currency.sln**: Solution file for the project.
- **README.md**: Project documentation.
- **coverlet.runsettings**: Code coverage configuration.
- **test-report-generator.bat**: Batch script for generating test reports.
- **coverage-report/**: Generated code coverage reports.
- **test-results/**: Test result outputs.
- **CurrencyAPI/**: Main API project (controllers, startup, etc.).
- **CurrencyAPI.Tests/**: Unit and integration tests.
- **CurrencyApp.Application/**: Application logic, CQRS handlers, validators.
- **CurrencyApp.Data/**: Data models, repositories, specifications.
- **CurrencyApp.Infra/**: Infrastructure services, external API providers.