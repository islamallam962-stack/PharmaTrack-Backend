using System.Net;
using System.Text.Json;

namespace ApiGateway.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate              _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next,
                               ILogger<ExceptionMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in Gateway");
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode  = (int)HttpStatusCode.InternalServerError;
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                success = false,
                message = "Gateway error occurred."
            }));
        }
    }
}