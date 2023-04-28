using System.Net;
using netdetective.Utils;

namespace netdetective.IpTests;

// Named after https://github.com/X4BNet/lists_vpn
public class X4BNetLists
{
    private readonly ILogger<X4BNetLists> _logger;
    private readonly FileChanged _vpnListFile;
    private readonly FileChanged _datacenterListFile;
    private List<IPNetwork> _vpnNetworks = new();
    private List<IPNetwork> _dataCenterNetworks = new();

    public X4BNetLists(ILogger<X4BNetLists> logger, string vpnListPath, string datacenterListPath)
    {
        _logger = logger;;
        _vpnListFile = new FileChanged(vpnListPath);
        _datacenterListFile = new FileChanged(datacenterListPath);
    }

    public async Task<X4BNetListsFlags> Query(IPAddress ipAddress)
    {
        var result = X4BNetListsFlags.None;

        if (_vpnListFile.LastWriteTimeHasChanged(true))
        {
            _vpnNetworks = await ReadNetworks(_vpnListFile.Path);
        }

        if (_vpnNetworks.Any(x => x.Contains(ipAddress)))
        {
            result |= X4BNetListsFlags.Vpn;
        }

        if (_datacenterListFile.LastWriteTimeHasChanged(true))
        {
            _dataCenterNetworks = await ReadNetworks(_datacenterListFile.Path);
        }

        if (_dataCenterNetworks.Any(x => x.Contains(ipAddress)))
        {
            result |= X4BNetListsFlags.DataCenter;
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
