using System.Net;

namespace netdetective;

public class IPNetwork
{
    private readonly UInt32 _networkAddress;
    private readonly UInt32 _subnetMask;

    public UInt64 Length => (UInt64) (~_subnetMask) + 1;

    public IPNetwork(string cidrNotation)
    {
        var parts = cidrNotation.Split('/');
        if (parts.Length != 2)
        {
            throw new FormatException("Incorrect CIDR notation");
        }

        if (!IPAddress.TryParse(parts[0], out var ipAddress))
        {
            throw new FormatException($"Failed to parse IP address \"{parts[0]}\"");
        }

        if (!int.TryParse(parts[1], out var prefixLength))
        {
            throw new FormatException($"Failed to parse prefix length \"{parts[1]}\"");
        }

        _networkAddress = IpAddressToInteger(ipAddress);
        _subnetMask = PrefixToMask(prefixLength);

        if ((_networkAddress & _subnetMask) != _networkAddress)
        {
            throw new FormatException("Network address bits outside of prefix");
        }
    }

    public bool Contains(IPAddress ipAddress) =>
        (IpAddressToInteger(ipAddress) & _subnetMask) == _networkAddress;

    private static UInt32 IpAddressToInteger(IPAddress ipAddress) => BitConverter.IsLittleEndian
        ? BitConverter.ToUInt32(ipAddress.GetAddressBytes().Reverse().ToArray())
        : BitConverter.ToUInt32(ipAddress.GetAddressBytes());

    private static UInt32 PrefixToMask(int prefixLength) => prefixLength == 0 ? 0 : ~(((UInt32) 1 << (32 - prefixLength)) - 1);
}
