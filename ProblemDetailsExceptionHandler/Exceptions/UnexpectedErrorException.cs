using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler.Exceptions;

/// <summary>
///     Thrown when an unexpected error happens, usually with status code <c>500 - Internal server error</c>.
/// </summary>
public sealed class UnexpectedErrorException : ProblemDetailsException
{
    public UnexpectedErrorException(int status, string title, string detail, Exception? innerException = null)
            : base(status, title, detail, innerException) { }

    public UnexpectedErrorException(string title, string detail, Exception? innerException = null)
            : base(500, title, detail, innerException) { }

    /// <inheritdoc />
    public UnexpectedErrorException(ProblemDetails     problemDetails,
                                    IHeaderDictionary? headers        = null,
                                    Exception?         innerException = null)
            : base(problemDetails, headers, innerException) { }
}
