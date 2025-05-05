# Currency App

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

## Environments
Development, Test, Production

**Note:** Use the **Development** environment to test locally.

## Test Accounts

| User             | Password    | Roles                    |
|------------------|-------------|--------------------------|
| admin@app.com    | Admin@123   | Admin, Customer, Analyst |
| varun@app.com    | Varun@123   | Customer                 |
| teja@app.com     | Teja@123    | Customer, Analyst        |

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
- Output Caching
- Rate Limiting

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

## Exchange Rates Flow

```
[ExchangeRates-Endpoint]
        ↓
[ExchangeRatesHandler]
        ↓
[CurrencyProviderFactory]
        ↓
[FrankfurterProvider]
        ↓
[IFrankfurterApi]
```

**Description:**
1. **(ExchangeRates - endpoint)** receives the request for exchange rates.
2. **ExchangeRatesHandler (MediatR)** processes the request and coordinates the application logic.
3. **CurrencyProviderFactory** selects the appropriate provider (e.g., FrankfurterProvider).
4. **FrankfurterProvider** implements the logic to interact with the external API and cache management.
5. **IFrankfurterApi (Refit)** is the Refit HTTP client interface for calling the Frankfurter exchange rate service.

## Login Flow
**Description:**
1. **UsersController -> Login** receives the login request from the client.
2. **LoginRequestValidator** validates the login request data.
3. **LoginRequestHandler** processes the login logic, validating credentials.
4. **IUnitOfWork** manages transactional operations and coordinates repository actions.
5. **IRepository** is used to fetch user data from the data store.
6. **ITokenService** generates authentication tokens for the user upon successful login.

## Assumptions Made
- Admin can manage roles of other users
- In future, can switch to any currency exchange rate provider
- Other currency providers supports pagination for historical data, so added caching at provider level for Frankfurter provider

## Possible Future Enhancements
- Distributed caching
- Reverse proxy
- Two Factor Authentication (2FA)
- Audit logging
- Health checks