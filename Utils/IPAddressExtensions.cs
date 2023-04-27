using System.Net;

namespace netdetective;

public static class IPAddressExtensions
{
    private static string ReverseIp4Address(IPAddress ipAddress) =>
        string.Join('.', ipAddress.GetAddressBytes().Reverse());

    private static string ReverseIp6Address(IPAddress ipAddress) =>
        string.Join('.', ipAddress.GetAddressBytes()
            .Reverse()
            .ToArray()
            .SelectMany(x => x.ToString("x2").Reverse()));

    public static string ReverseIpAddress(this IPAddress ipAddress)
    {
        switch (ipAddress.AddressFamily)
        {
            case System.Net.Sockets.AddressFamily.InterNetwork:
                return ReverseIp4Address(ipAddress);
            case System.Net.Sockets.AddressFamily.InterNetworkV6:
                return ReverseIp6Address(ipAddress);
            default:
                throw new NotImplementedException("Only IPV4 and IPV6 are supported");
        }
    }

    public static string ToReverseDns(this IPAddress ipAddress)
    {
        switch (ipAddress.AddressFamily)
        {
            case System.Net.Sockets.AddressFamily.InterNetwork:
                return ReverseIp4Address(ipAddress) + ".in-addr.arpa";
            case System.Net.Sockets.AddressFamily.InterNetworkV6:
                return ReverseIp6Address(ipAddress) + ".ip6.arpa";
            default:
                throw new NotImplementedException("Only IPV4 and IPV6 are supported");
        }
    }
}
