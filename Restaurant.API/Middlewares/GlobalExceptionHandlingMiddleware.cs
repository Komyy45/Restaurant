using System.Text;
using System.Text.Json;

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
            context.Response.ContentType = "application/json; charset=utf-8";
            var json = JsonSerializer.Serialize(new { statusCode = StatusCodes.Status500InternalServerError, message = e.Message });
            var bytes = Encoding.UTF8.GetBytes(json);
            await context.Response.Body.WriteAsync(bytes);
        }
    }
}