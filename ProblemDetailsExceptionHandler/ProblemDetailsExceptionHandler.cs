using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler.Exceptions;

namespace Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler;

/// <summary>
///     Handles exceptions so they will be returned as ProblemDetails responses.
///     Exceptions derived from <see cref="ProblemDetailsException" /> will be
///     directly used to obtain the response.
///     Exceptions whose types are registered will be resolved to obtain a
///     <see cref="ProblemDetailsException" />.
///     Any other exception will use de default values stored in the exception
///     register.
/// </summary>
/// <remarks>
///     The value of the resulting ProblemDetails Status property will be set
///     to <c>500</c> if none is set.
///     The value of the resulting ProblemDetails Type property will be inferred
///     based on the Status property if it has
///     none.
/// </remarks>
/// <param name="logger">The logger used to log the exceptions.</param>
/// <param name="exceptionRegistrySnapshotOptions">
///     The exception registry used to resolve registered exception types.
/// </param>
public class ProblemDetailsExceptionHandler(ILogger<ProblemDetailsExceptionHandler> logger,
                                            IOptions<ExceptionRegistry>             exceptionRegistrySnapshotOptions)
        : IExceptionHandler
{
    #region IExceptionHandler Members

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext       httpContext,
                                                Exception         exception,
                                                CancellationToken cancellationToken)
    {
        var registry = exceptionRegistrySnapshotOptions.Value;
        var error = exception as ProblemDetailsException
                 ?? registry.Resolve(exception);
        var problemDetails = error?.ProblemDetails ?? registry.DefaultProblemDetails;

        problemDetails.Status ??= 500;
        problemDetails.Type   ??= registry.CreateTypeUrlFromStatus(problemDetails.Status.Value);

        logger.Log(LogLevel.Error,
                   error ?? exception,
                   "({Status}) - {Title} - {Detail}",
                   problemDetails.Status,
                   problemDetails.Title,
                   problemDetails.Detail);

        httpContext.Response.StatusCode = problemDetails.Status ?? 500;

        foreach (var (key, value) in error?.Headers ?? registry.DefaultHeaders)
        {
            httpContext.Response.Headers[key] = value;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails,
                                                    JsonSerializerOptions.Default,
                                                    MediaTypeNames.Application.ProblemJson,
                                                    cancellationToken);

        return true;
    }

    #endregion
}
