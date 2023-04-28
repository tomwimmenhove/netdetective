using System.Net;
using DnsClient;
using DnsClient.Protocol;
using netdetective.Utils;

namespace netdetective.BlackList;

public class DnsBl
{
    private readonly LookupClient _client = new LookupClient(new LookupClientOptions { UseCache = true } );
    private readonly ILogger<DnsBl> _logger;
    private readonly FileChanged _serverListFile;
    private DnsBlServer[] _servers = Array.Empty<DnsBlServer>();

    public DnsBl(ILogger<DnsBl> logger, string dnsBlDatabasePath)
    {
        _logger = logger;
        _serverListFile = new FileChanged(dnsBlDatabasePath);
    }

    private async Task UpdateServerList()
    {
        if (!_serverListFile.LastWriteTimeHasChanged(true))
        {
            return;
        }

        _logger.LogInformation($"Reading DnsBl database {_serverListFile.Path}");
        var json = await File.ReadAllTextAsync(_serverListFile.Path);

        var servers = System.Text.Json.JsonSerializer.Deserialize<DnsBlServer[]>(json);
        if (servers == null)
        {
            throw new FormatException($"{_serverListFile.Path} was not in the correct format");
        }

        _servers = servers;
    }

    public async Task<DnsBlFlags> Query(IPAddress ipAddress, CancellationToken token)
    {
        await UpdateServerList();

        var testLookupTasks = _servers.Select(x => new
            {
                Server = x,
                Task = _client.QueryAsync(ipAddress
                    .ReverseIpAddress() + '.' + x.Server, QueryType.A, cancellationToken: token)
            }).ToList();

        var results = DnsBlFlags.None;
        while (testLookupTasks.Any())
        {
            var task = await Task.WhenAny(testLookupTasks.Select(x => x.Task));
            var entry = testLookupTasks.First(x => x.Task == task);
            testLookupTasks.Remove(entry);

            ARecord[] arecords;
            try
            {
                var result = await entry.Task;
                arecords = result.Answers.ARecords().ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get results from {entry.Server}: {e.Message}");
                continue;
            }

            foreach (var arecord in arecords)
            {
                if (entry.Server.Responses.TryGetValue(arecord.Address.ToString(), out var addFlags))
                {
                    results |= addFlags;
                }
            }
        }

        return results;
    }
}
