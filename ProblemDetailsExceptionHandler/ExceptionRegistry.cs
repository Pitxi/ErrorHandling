using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler.Exceptions;

namespace Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler;

/// <summary>
///     Exception resolvers registry and related options.
/// </summary>
public class ExceptionRegistry
{
    #region Delegates

    public delegate ProblemDetailsException ExceptionResolver<in TException>(TException ex)
            where TException : Exception;

    #endregion

    private readonly Dictionary<string, object> _registry = new();

    /// <summary>
    ///     The callback used to generate the value of the Type property of a
    ///     ProblemDetails object when it has no value.
    /// </summary>
    public Func<int, string?> CreateTypeUrlFromStatus = statusCode => $"https://httpstatuses.io/{statusCode}";

    /// <summary>
    ///     Default <see cref="ProblemDetails" /> object used by the
    ///     <see cref="ProblemDetailsExceptionHandler" /> exception handler.
    /// </summary>
    public ProblemDetails DefaultProblemDetails { get; } = new()
    {
            Status = StatusCodes.Status500InternalServerError,
            Title  = "Unexpected error",
            Detail = "An unexpected error occurred."
    };

    /// <summary>
    ///     Headers added to the <see cref="ProblemDetailsExceptionHandler" />
    ///     exception handler default response.
    /// </summary>
    /// <remarks>
    ///     These headers are a fallback and will not be used if the handled
    ///     exception is already a <see cref="ProblemDetailsException" />
    ///     subclass, in which case the exception's headers will be used.
    /// </remarks>
    public HeaderDictionary DefaultHeaders { get; } = new();

    /// <summary>
    ///     Registers a new exception resolver.
    /// </summary>
    /// <param name="resolver">The exception resolver to be registered.</param>
    /// <typeparam name="TException">
    ///     Type of the exceptions being resolved by the exception resolver.
    /// </typeparam>
    /// <returns>
    ///     The current <see cref="ExceptionRegistry" /> instance.
    /// </returns>
    public ExceptionRegistry Register<TException>(ExceptionResolver<TException> resolver)
            where TException : Exception
    {
        var paramType = resolver.GetType().GetMethod("Invoke")?.GetParameters().Single().ParameterType!;

        _registry.Add(paramType.FullName!, resolver);

        return this;
    }

    /// <summary>
    ///     Unregisters an exception resolver.
    /// </summary>
    /// <typeparam name="TException">
    ///     The exception type associated to the exception resolver to be
    ///     unregistered.
    /// </typeparam>
    /// <returns>
    ///     The current <see cref="ExceptionRegistry" /> instance.
    /// </returns>
    public ExceptionRegistry Unregister<TException>() where TException : Exception
    {
        _registry.Remove(typeof(TException).FullName!);

        return this;
    }

    /// <summary>
    ///     Resolves the given exception.
    /// </summary>
    /// <param name="exception">The exception to be resolved.</param>
    /// <returns>
    ///     The resulting <see cref="ProblemDetailsException" />, or null if no
    ///     resolver was registered for the input exception's type.
    /// </returns>
    public ProblemDetailsException? Resolve(Exception exception)
    {
        _registry.TryGetValue(exception.GetType().FullName!, out dynamic? resolver);

        return resolver?.Invoke(exception as dynamic);
    }
}
