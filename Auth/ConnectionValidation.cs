using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using netdetective.Dtos;
using netdetective.RequestInfoProviders;

namespace netdetective.Auth;

public class ConnectionValidation
{
    private readonly RequestDelegate _next;
    private readonly ConnectionValidationSettings _settings;

    public ConnectionValidation(RequestDelegate next,IOptions<ConnectionValidationSettings> settings)
    {
        _next = next;
        _settings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var requestInfoProvider = scope.ServiceProvider.GetRequiredService<IRapidApiRequestInfoProvider>();

            if (_settings.SecretValue != null && requestInfoProvider.GetSecret() != _settings.SecretValue)
            {
                var json = JsonSerializer.Serialize(new SimpleErrorResponeDto { Message = "Unauthorized" });
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json, Encoding.UTF8);

                return;
            }
        }

        await _next(context);
    }
}
