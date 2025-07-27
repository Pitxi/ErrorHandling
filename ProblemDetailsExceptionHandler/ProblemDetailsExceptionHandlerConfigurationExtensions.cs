using Microsoft.Extensions.DependencyInjection;

namespace Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler;

public static class ProblemDetailsExceptionHandlerConfigurationExtensions
{
    public static IServiceCollection AddProblemDetailsExceptionHandler(this IServiceCollection   services,
                                                                       Action<ExceptionRegistry> configure)
    {
        return services.Configure(configure)
                       .AddProblemDetails()
                       .AddExceptionHandler<ProblemDetailsExceptionHandler>();
    }

    public static IServiceCollection AddProblemDetailsExceptionHandler(this IServiceCollection services)
    {
        return services.AddProblemDetailsExceptionHandler(_ => { });
    }
}
