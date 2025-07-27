using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler.Exceptions;

/// <summary>
///     Base class for all ProblemDetails exceptions.
/// </summary>
public abstract class ProblemDetailsException : Exception
{
    protected ProblemDetailsException(
            ProblemDetails     problemDetails,
            IHeaderDictionary? headers        = null,
            Exception?         innerException = null)
            : base($"({problemDetails.Status}) - {problemDetails.Title}\n{problemDetails.Detail}")
    {
        ProblemDetails = problemDetails;
        Headers        = headers ?? new HeaderDictionary();
    }

    protected ProblemDetailsException(
            int                           status,
            string?                       type,
            string                        title,
            string                        detail,
            string?                       instance,
            IDictionary<string, object?>? extensions,
            IHeaderDictionary?            headers,
            Exception?                    innerException = null)
            : base($"({status}) - {title}\n{detail}", innerException)
    {
        ProblemDetails = new ProblemDetails
        {
                Status   = status,
                Type     = type,
                Title    = title,
                Detail   = detail,
                Instance = instance
        };

        ProblemDetails.Extensions = extensions ?? ProblemDetails.Extensions;
        Headers                   = headers    ?? new HeaderDictionary();
    }

    protected ProblemDetailsException(
            int        status,
            string?    type,
            string     title,
            string     detail,
            string?    instance       = null,
            Exception? innerException = null)
            : this(status, type, title, detail, instance, null, null, innerException) { }

    protected ProblemDetailsException(
            int        status,
            string     title,
            string     detail,
            string     instance,
            Exception? innerException = null)
            : this(status, null, title, detail, instance, null, null, innerException) { }

    protected ProblemDetailsException(
            int        status,
            string     title,
            string     detail,
            Exception? innerException = null)
            : this(status, null, title, detail, null, null, null, innerException) { }

    public IHeaderDictionary Headers        { get; }
    public ProblemDetails    ProblemDetails { get; }
}
