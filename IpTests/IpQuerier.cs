using Microsoft.Extensions.Options;
using System.Net;
using netdetective.BlackList;

namespace netdetective.IpTests;

public class IpQuerier : IIpQuerier
{
    private readonly X4BNetLists _listsVpn;
    private readonly DnsBl _dnsBl;

    public IpQuerier(IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var settings = serviceScope.ServiceProvider.GetRequiredService<IOptions<IpInfoSettings>>();

        _listsVpn = ActivatorUtilities.CreateInstance<X4BNetLists>(serviceScope.ServiceProvider,
            settings.Value.VpnListPath, settings.Value.DatacenterListPath);
        _dnsBl = ActivatorUtilities.CreateInstance<DnsBl>(serviceScope.ServiceProvider,
            settings.Value.DnsBlDatabasePath);
    }

    public async Task<IpInfoResults> Query(string ipAddress, CancellationToken token)
    {
        if (!IPAddress.TryParse(ipAddress, out var address))
        {
            throw new FormatException($"Failed to parse IP address");
        }

        if (address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork &&
            address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
        {
            throw new FormatException($"Invalid IP address family");
        }

        var listVpnResults = await _listsVpn.Query(address);
        var dnsBlResults = await _dnsBl.Query(address, token);

        return new IpInfoResults(address.ToString(), listVpnResults, dnsBlResults);
    }
}
