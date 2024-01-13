using System.Net;
using System.Text.Json;
using RainfallApi.DTOs;

namespace RainfallApi.Middleware;

public class ErrorResponseMiddleware
{
    private readonly ILogger<ErrorResponseMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorResponseMiddleware(RequestDelegate next, ILogger<ErrorResponseMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new Error
                {
                    Message = $"Link not found",
                    Detail = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        PropertyName = "Link not found",
                        Message = "You may have mistyped the url"
                    }
                }
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new Error
            {
                Message = $"{context.Response.StatusCode} Internal Server Error",
                Detail = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        PropertyName = "Server",
                        Message = $"{ex.Message} Stack Trace: {ex.StackTrace?.ToString()}"
                    }
                }
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
