# Klinkby.CleanFn

Middleware for Azure Functions v4 isolated worker model (dotnet 8.0 LTS) to decouple
application logic from the Azure Functions runtime and provide opinionated handling
of boilerplate stuff in REST APIs.

MIT Licensed.

## Features

- Simple mediator dispatch request object to application logic that may reply with response object
    - Optional implicit request validation using DataAnnotations or subclass the basic mediator
      to roll your own validation
- Echos `X-Correlation-Id` header
    - Correlation Id is also added to outgoing HttpClient request headers
- Enables Application Insights telemetry when `APPINSIGHTS_INSTRUMENTATIONKEY` is set
    - Adds Correlation Id to custom telemetry property
- Map configured dotnet exceptions to friendly `application/problem+json` responses
- Support writing `application/health+json` formatted health check
- Sends Mozilla recommended security response headers
- Reduced cold start latency
    - Demand source generation for JSON-serialized request/response objects
    - Internal logging via source generation
- Application logic only reference netstandard nuget with stable abstract interfaces
- Use `System.Text.Json` for serialization

## Inspiration

- [MS Isolated Process Guide](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide)
- [@Jbogard's MediatR](https://github.com/jbogard/MediatR)
- [@Khellang's ProblemDetails](https://github.com/khellang/Middleware/tree/master/src/ProblemDetails)
- [Mozilla's HTTP Observatory](https://observatory.mozilla.org/faq/)