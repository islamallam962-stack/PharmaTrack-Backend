using FluentValidation;
using NotificationService.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace NotificationService.API.Middleware;

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
        try { await _next(ctx); }
        catch (DomainException ex)
        {
            await WriteError(ctx, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
            await WriteError(ctx, HttpStatusCode.UnprocessableEntity,
                             "Validation failed.", errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteError(ctx, HttpStatusCode.InternalServerError,
                             "An unexpected error occurred.");
        }
    }

    private static Task WriteError(HttpContext ctx, HttpStatusCode status,
                                   string message, List<string>? errors = null)
    {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode  = (int)status;
        return ctx.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            success = false, message,
            errors  = errors ?? new List<string>()
        }));
    }
}