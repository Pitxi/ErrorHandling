# ProblemDetails exception handler

![Static Badge](https://img.shields.io/badge/Library-ASP.NET_Core_9-blue?logo=dotnet)
![Static Badge](https://img.shields.io/badge/Assembly_name-Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler-darkgreen)
![Static Badge](https://img.shields.io/badge/Version-9.0.0-green)

Exception handler that produces `application/problem+json` responses.

## Functionality

* Handles exceptions derived from the `ProblemDetailsException` base class
  returning an `application/problem+json` response.
* Optionally, allows registering lambdas to resolve exceptions into
  `ProblemDetailsException` derived exceptions in the exception handler.
* Allows to change the values of `ProblemDetailsException` used in the
  exception handler by default.
* Allows to change the callback function used to set the `ProblemDetails.Type`
  property based in its status code.

## Usage

```C#
// ...

// Adding the exception handler to the services and registering an exception
// resolver
builder.Services.AddProblemDetailsExceptionHandler(registry => 
{
    // Setting default values for fallback.
    registry.DefaultProblemDetails.Title  = "Error inesperado";
    registry.DefaultProblemDetails.Detail = "Ocurrió un error inesperado.";

    // Setting default headers for fallback.
    registry.DefaultHeaders["X-Test"] = "Testing header content";

    // Setting type property URL creation callback. By default the library uses
    // https://httpstatuses.io base URL.
    registry.CreateTypeUrlFromStatus = status => $"https://http.dev/{status}";

    // Any unhandled ArgumentNullException thrown will be transformed into a
    // MyProblemDetailsException in the exception handler.
    registry.Add<ArgumentNullException>(ex =>
        new MyProblemDetailsException(
            "Argument is null",
            $"Argument '{ex.PAramName}' cannot be null.",
            ex
        ));
});

// ...

var app = builder.Build();

app.UseExceptionHandler();

// ...

app.Run();
```
