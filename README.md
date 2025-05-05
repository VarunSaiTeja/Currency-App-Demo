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