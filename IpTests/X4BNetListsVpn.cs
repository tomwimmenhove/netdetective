using System.Net;

namespace netdetective.IpTests;

// Named after https://github.com/X4BNet/lists_vpn
public class X4BNetListsVpn
{
    private List<IPNetwork> _vpnNetworks;
    private List<IPNetwork> _dataCenterNetworks;

    private X4BNetListsVpn(List<IPNetwork> vpnNetworks, List<IPNetwork> dataCenterNetworks)
    {
        _vpnNetworks = vpnNetworks;
        _dataCenterNetworks = dataCenterNetworks;
    }

    public static async Task<X4BNetListsVpn> Create(string vpnListPath, string datacenterListPath)
    {
        var vpnNetworks = await ReadNetworks(vpnListPath);
        var dataCenterNetworks = await ReadNetworks(datacenterListPath);

        return new X4BNetListsVpn(vpnNetworks, dataCenterNetworks);
    }

    public X4BNetListsVpnResults Test(IPAddress ipAddress)
    {
        var result = X4BNetListsVpnResults.None;

        if (_vpnNetworks.Any(x => x.Contains(ipAddress)))
        {
            result |= X4BNetListsVpnResults.Vpn;
        }

        if (_dataCenterNetworks.Any(x => x.Contains(ipAddress)))
        {
            result |= X4BNetListsVpnResults.DataCenter;
        }

        return result;
    }

    public bool IsVpn(IPAddress ipAddress) => _vpnNetworks.Any(x => x.Contains(ipAddress));
    public bool IsDataCenter(IPAddress ipAddress) => _dataCenterNetworks.Any(x => x.Contains(ipAddress));

    private static async Task<List<IPNetwork>> ReadNetworks(string path)
    {
        var lines = await File.ReadAllLinesAsync(path);

        var allNetworks = new List<IPNetwork>();
        foreach (var line in lines)
        {
            try
            {
                allNetworks.Add(new IPNetwork(line));
            }
            catch (FormatException e)
            {
                Console.Error.WriteLine($"Adding network \"{line}\" failed: {e.Message}");
            }
        }

        return allNetworks;
    }
}
