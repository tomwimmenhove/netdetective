using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using netdetective.Controllers;

namespace netdetective.Auth;

public class ConnectionValidation
{
    private readonly RequestDelegate _next;
    private readonly ConnectionValidationSettings _settings;

    public ConnectionValidation(RequestDelegate next, IOptions<ConnectionValidationSettings> settings)
    {
        _next = next;
        _settings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!string.IsNullOrEmpty(_settings.SecretHeader) &&
            context.Request.Headers[_settings.SecretHeader].FirstOrDefault() != _settings.SecretValue)
        {
            var json = JsonSerializer.Serialize(new SimpleErrorResponeDto { Message = "Unauthorized" });
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json, Encoding.UTF8);
        }
        else
        {
            await _next(context);
        }
    }
}
