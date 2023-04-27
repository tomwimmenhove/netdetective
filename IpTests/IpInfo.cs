using Microsoft.Extensions.Options;
using System.Net;
using netdetective.BlackList;

namespace netdetective.IpTests;

public class IpInfo : IIpTest
{
    private readonly X4BNetListsVpn _listsVpn;
    private readonly DnsBl _dnsBl;

    private IpInfo(X4BNetListsVpn listsVpn, DnsBl dnsBl)
    {
        _listsVpn = listsVpn;
        _dnsBl = dnsBl;
    }

    public static async Task<IpInfo> Create(IOptions<IpInfoSettings> settings)
    {
        var listsVpn = await X4BNetListsVpn.Create(settings.Value.VpnListPath, settings.Value.DatacenterListPath);
        var dnsBl = await DnsBl.Create(settings.Value.DnsBlDatabasePath);

        return new IpInfo(listsVpn, dnsBl);
    }

    public async Task<IpInfoResults> Test(string ipAddress, CancellationToken token)
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

        var listVpnResults = _listsVpn.Test(address);
        var dnsBlResults = await _dnsBl.Test(address, token);

        return new IpInfoResults(address.ToString(), listVpnResults, dnsBlResults);
    }
}
