using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using netdetective.Dtos;
using netdetective.RequestInfoProviders;

namespace netdetective.Auth;

public class ConnectionValidation
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ConnectionValidation> _logger;
    private readonly ConnectionValidationSettings _settings;

    public ConnectionValidation(RequestDelegate next, ILogger<ConnectionValidation> logger,
        IOptions<ConnectionValidationSettings> settings)
    {
        _next = next;
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var requestInfoProvider = scope.ServiceProvider.GetRequiredService<IRapidApiRequestInfoProvider>();

        var hasAccess = _settings.SecretValue == null || requestInfoProvider.GetSecret() == _settings.SecretValue;
        LogRequest(context, requestInfoProvider, hasAccess);

        if (hasAccess)
        {
            await _next(context);
            return;
        }

        var json = JsonSerializer.Serialize(new SimpleErrorResponeDto { Message = "Unauthorized" });
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json, Encoding.UTF8);
    }

    private void LogRequest(HttpContext context, IRapidApiRequestInfoProvider requestInfoProvider, bool access)
    {
        var clientIps = string.Join(',', requestInfoProvider.GetClientAddresses());
        _logger.LogInformation($"{requestInfoProvider.GetUsername()}@{clientIps}: " +
            $"\"{context.Request.Path}{context.Request.QueryString}\" - {(access ? "Authorized" : "Unauthorized")}");
    }
}
