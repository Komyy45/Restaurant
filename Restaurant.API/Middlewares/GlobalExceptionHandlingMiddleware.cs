using System.Text;
using System.Text.Json;
using Restaurant.Domain.Exceptions;

namespace Restaurant.API.Middlewares;

internal sealed class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            object message;
            switch (e)
            {
                case NotFoundException:
                    message = new { statusCode = StatusCodes.Status500InternalServerError, message = e.Message };
                    break;
                case IsNotAllowedException:
                case AccountLockedException: 
                    message = new { statusCode = StatusCodes.Status403Forbidden, message = e.Message };
                    break;
                case ValidationException:
                    message = new { statusCode = StatusCodes.Status400BadRequest, message = e.Message };
                    break;
                default:
                    message = new { statusCode = StatusCodes.Status500InternalServerError, message = e.Message };
                    break;
            }
            context.Response.ContentType = "application/json; charset=utf-8";
            var json= JsonSerializer.Serialize(message);
            var bytes = Encoding.UTF8.GetBytes(json);
            await context.Response.Body.WriteAsync(bytes);
        }
    }
}