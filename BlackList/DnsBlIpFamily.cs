using System.Text.Json.Serialization;

namespace netdetective.BlackList;

[Flags]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DnsBlIpFamily
{
    Ipv4 = 1,
    Ipv6 = 2,
    Both = 3
}
