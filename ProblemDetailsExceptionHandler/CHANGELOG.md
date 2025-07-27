# Changelog

## [9.0.0] - 2025-07-26

### Added

- Add Exceptions
    - `ProblemDetailsException`
    - `UnexpectedErrorException`
- Add exception handler `ProblemDetailsExceptionHandler`
- Add exception resolver registry `ExceptionRegistry`
    - Allow registering exception resolvers.
    - Allow unregistering exception resolvers.
    - Allow executing exception resolvers.
    - Allow setting default values.
    - Allow setting type URL generator callback.
- Add configuration and dependency injection registration.
