namespace netdetective.RequestInfoProviders;

public class RapidApiRequestInfoProvider : RequestInfoProvider, IRapidApiRequestInfoProvider
{
    public RapidApiRequestInfoProvider(IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    { }

    public string? GetSecret() =>
        HttpContextAccessor.HttpContext?.Request.Headers["X-RapidAPI-Proxy-Secret"].FirstOrDefault();

    public string? GetUsername() =>
        HttpContextAccessor.HttpContext?.Request.Headers["X-RapidAPI-User"].FirstOrDefault();
}
