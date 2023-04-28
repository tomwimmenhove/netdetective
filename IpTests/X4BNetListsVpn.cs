using System.Net;
using netdetective.Utils;

namespace netdetective.IpTests;

// Named after https://github.com/X4BNet/lists_vpn
public class X4BNetListsVpn
{
    private readonly ILogger _logger;
    private readonly FileLatest _vpnListFile;
    private readonly FileLatest _datacenterListFile;
    private List<IPNetwork> _vpnNetworks = new();
    private List<IPNetwork> _dataCenterNetworks = new();

    public X4BNetListsVpn(ILogger logger, string vpnListPath, string datacenterListPath)
    {
        _logger = logger;
        _vpnListFile = new FileLatest(vpnListPath);
        _datacenterListFile = new FileLatest(datacenterListPath);
    }

    public async Task<X4BNetListsVpnResults> Test(IPAddress ipAddress)
    {
        var result = X4BNetListsVpnResults.None;

        if (_vpnListFile.HasNewWrites(true))
        {
            _vpnNetworks = await ReadNetworks(_vpnListFile.Path);
        }

        if (_vpnNetworks.Any(x => x.Contains(ipAddress)))
        {
            result |= X4BNetListsVpnResults.Vpn;
        }

        if (_datacenterListFile.HasNewWrites(true))
        {
            _dataCenterNetworks = await ReadNetworks(_datacenterListFile.Path);
        }

        if (_dataCenterNetworks.Any(x => x.Contains(ipAddress)))
        {
            result |= X4BNetListsVpnResults.DataCenter;
        }

        return result;
    }

    private static FileSystemWatcher CreateFileSystemWatcher(string fullPath) => new FileSystemWatcher
    {
        Path = Path.GetDirectoryName(fullPath)!,
        Filter = Path.GetFileName(fullPath),
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true
    };

    private async Task<List<IPNetwork>> ReadNetworks(string path)
    {
        _logger.LogInformation($"Reading X4BNetListsVpn database {path}");
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
                _logger.LogError($"Adding network \"{line}\" failed: {e.Message}");
            }
        }

        return allNetworks;
    }
}
