namespace netdetective.BlackList;

public class DnsBlServer
{
    public string Server { get; }
    public string Name { get; }
    public DnsBlIpFamily IpFamily { get; }
    public IDictionary<string, DnsBlFlags> Responses { get; }

    public DnsBlServer(string server, string name, DnsBlIpFamily ipFamily, IDictionary<string, DnsBlFlags> responses)
    {
        Server = server;
        Name = name;
        IpFamily = ipFamily;
        Responses = responses;
    }
}