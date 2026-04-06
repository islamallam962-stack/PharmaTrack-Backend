namespace ApiGateway.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate              _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next,
                                    ILogger<RequestLoggingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var start = DateTime.UtcNow;

        _logger.LogInformation(
            "[Gateway] {Method} {Path} — started",
            ctx.Request.Method,
            ctx.Request.Path);

        await _next(ctx);

        var elapsed = (DateTime.UtcNow - start).TotalMilliseconds;

        _logger.LogInformation(
            "[Gateway] {Method} {Path} — {StatusCode} ({Elapsed}ms)",
            ctx.Request.Method,
            ctx.Request.Path,
            ctx.Response.StatusCode,
            elapsed);
    }
}