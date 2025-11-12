using System.Net;
using System.Text.Json;
using ExpenseTracker.Application.Models;

namespace ExpenseTracker.Api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{   
    private static readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodes = new()
    {
        { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
        { typeof(ArgumentException), HttpStatusCode.BadRequest },
        { typeof(KeyNotFoundException), HttpStatusCode.BadRequest },
        { typeof(Exception), HttpStatusCode.InternalServerError }
    };
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = _exceptionStatusCodes
            .GetValueOrDefault(exception.GetType(), HttpStatusCode.InternalServerError);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = Response<object>.Fail(exception.Message, (int)statusCode);
        
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions 
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}