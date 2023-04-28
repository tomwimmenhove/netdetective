using System.Net;
using DnsClient;

namespace netdetective.BlackList;

public class DnsBl
{
    private readonly LookupClient _client = new LookupClient(new LookupClientOptions { UseCache = true } );
    private readonly IEnumerable<DnsBlServer> _servers;

    private DnsBl(IEnumerable<DnsBlServer> servers)
    {
        _servers = servers;
    }

    public static async Task<DnsBl> Create(ILogger logger, string dnsBlDatabasePath)
    {
        logger.LogInformation($"Reading DnsBl database {dnsBlDatabasePath}");
        var json = await File.ReadAllTextAsync(dnsBlDatabasePath);

        var servers = System.Text.Json.JsonSerializer.Deserialize<DnsBlServer[]>(json);
        if (servers == null)
        {
            throw new FormatException($"{dnsBlDatabasePath} was not in the correct format");
        }

        return new DnsBl(servers);
    }

    public async Task<DnsBlFlags> Test(string ipAddress, CancellationToken token) =>
        await Test(IPAddress.Parse(ipAddress), token);

    public async Task<DnsBlFlags> Test(IPAddress ipAddress, CancellationToken token)
    {
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

            if (task.Status == TaskStatus.RanToCompletion)
            {
                var arecords = task.Result.Answers.ARecords().ToArray();
                foreach(var arecord in arecords)
                {
                    if (entry.Server.Responses.TryGetValue(arecord.Address.ToString(), out var addFlags))
                    {
                        results |= addFlags;
                    }
                }
            }

            testLookupTasks.Remove(entry);
        }

        return results;
    }
}
