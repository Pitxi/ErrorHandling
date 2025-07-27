using Microsoft.AspNetCore.Mvc;
using Pitxi.AspNetCore.ErrorHandling.ProblemDetailsExceptionHandler;
using Pitxi.AspNetCore.ErrorHandling.Testing.API.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddOpenApi()
       .AddProblemDetailsExceptionHandler(registry =>
       {
           registry.DefaultProblemDetails.Title  = "Error inesperado";
           registry.DefaultProblemDetails.Detail = "Ocurrió un error inesperado.";
           registry.DefaultHeaders["X-Test"]     = "Cabecera de prueba.";
           //registry.CreateTypeUrlFromStatus      = status => $"https://http.dev/{status}";

           registry.Register<InvalidOperationException>(ex => new TestException(ex));
       });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("test",
           string ([ FromQuery ] bool? throwException) =>
           {
               if (throwException is true)
               {
                   throw new InvalidOperationException();
               }

               return "Test finished";
           })
   .WithName("Test")
   .WithOpenApi();

app.UseExceptionHandler();
app.Run();
