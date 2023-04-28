using System.Net;

namespace netdetective.RequestInfoProviders;

public class RequestInfoProvider : IRequestInfoProvider
{
    protected IHttpContextAccessor HttpContextAccessor { get; private set; }

    public RequestInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public IEnumerable<IPAddress> GetClientAddresses()
    {
        if (HttpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException("No HttpContext");
        }

        var forwarded = HttpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (forwarded != null)
        {
            foreach(var ipString in forwarded.Split(','))
            {
                if (IPAddress.TryParse(ipString, out var ipAddress))
                {
                    yield return ipAddress;
                }
            }
        }

        if (HttpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
        {
            yield return HttpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        }
    }
}
