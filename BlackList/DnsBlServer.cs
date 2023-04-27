using System.Text.Json.Serialization;

namespace netdetective.BlackList;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DnsBlResponseType
{
    None = 0,
    BruteForce = 1 << 0,
    Spam = 1 << 1,
    Bogon = 1 << 2,
    ProxyHttp = 1 << 3,
    ProxySocks = 1 << 4,
    ProxyWeb = 1 << 13,
    ProxyOther = 1 << 5,
    SmtpRelay = 1 << 6,
    WebVuln = 1 << 7,
    NoMail = 1 << 8,
    Zombie = 1 << 9,
    PotentialZombie = 1 << 18,
    Dynamic = 1 << 10,
    NoServer = 1 << 11,
    BadConf = 1 << 12,
    DDos = 1 << 14,
    OpenDns = 1 << 15,
    Compromised = 1 << 16,
    Worm = 1 << 17,
    IrcDrone = 1 << 19,
    NoSpam = 1 << 20,
}

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DnsBlIpFamily
{
    Ipv4 = 1,
    Ipv6 = 2,
    Both = 3
}

public class DnsBlServer
{
    public string Server { get; }
    public string Name { get; }
    public DnsBlIpFamily IpFamily { get; }
    public IDictionary<string, DnsBlResponseType> Responses { get; }

    public DnsBlServer(string server, string name, DnsBlIpFamily ipFamily, IDictionary<string, DnsBlResponseType> responses)
    {
        Server = server;
        Name = name;
        IpFamily = ipFamily;
        Responses = responses;
    }
}