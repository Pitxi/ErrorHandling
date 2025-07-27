using Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler.Exceptions;

namespace Pitxi.AspNetCore.ErrorHandling.Testing.API.Exceptions;

public class TestException(Exception? innerException = null)
        : ProblemDetailsException(400,
                                  "Testing",
                                  "This is a testing ProblemDetails Exception",
                                  innerException);
