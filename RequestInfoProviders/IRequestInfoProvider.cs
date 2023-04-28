using System.Net;

namespace netdetective.RequestInfoProviders;

public interface IRequestInfoProvider
{
    IEnumerable<IPAddress> GetClientAddresses();
}
