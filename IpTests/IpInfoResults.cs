using netdetective.BlackList;
using System.ComponentModel;

namespace netdetective.IpTests;

public class IpInfoResults
{
    [Description("The IP address being queried")]
    public string IpAddress {get; }

    [Description("The IP address is a VPN")]
    public bool IsVpn { get; }
    
    [Description("Indicates whether the IP address belongs to a data center.")]
    public bool IsDataCenter { get; }

    [Description("The IP address is known for brute-force attacks.")]
    public bool IsBruteForce { get; }
    
    [Description("The IP address is known for sending spam.")]
    public bool IsSpam { get; }
    
    [Description("The IP address is a bogon (i.e., it should not appear in the public Internet routing table).")]
    public bool IsBogon { get; }
    
    [Description("The IP address is an open HTTP proxy.")]
    public bool IsProxyHttp { get; }
    
    [Description("The IP address is an open SOCKS proxy.")]
    public bool IsProxySocks { get; }

    [Description("The IP address is a web proxy.")]
    public bool IsProxyWeb { get; }

    [Description("The IP address is a type of proxy.")]
    public bool IsProxyOther { get; }
    
    [Description("The IP address is used to relay SMTP traffic.")]
    public bool IsSmtpRelay { get; }
    
    [Description("The IP address is vulnerable webserver that can be used for abuse.")]
    public bool IsWebVuln { get; }
    
    [Description("The IP address should not be used for email.")]
    public bool IsNoMail { get; }
    
    [Description("The IP address is a zombie (i.e., part of a botnet).")]
    public bool IsZombie { get; }

    [Description("The IP address is a potential bot.")]
    public bool IsPotentialZombie { get; }
    
    [Description("The IP address is dynamically assigned.")]
    public bool IsDynamic { get; }
    
    [Description("The IP address is not associated with a server.")]
    public bool IsNoServer { get; }
    
    [Description("The IP address is a misconfigured server.")]
    public bool IsBadConf { get; }
        
    [Description("The IP address is known for DDoS attacks.")]
    public bool IsDDos { get; }
    
    [Description("The IP address belongs to an OpenDNS resolver.")]
    public bool IsOpenDns { get; }
    
    [Description("The IP address has been compromised.")]
    public bool IsCompromised { get; }
    
    [Description("The IP address is part of a worm infection.")]
    public bool IsWorm { get; }
        
    [Description("The IP address is an IRC drone")]
    public bool IsIrcDrone { get; }
    
    [Description("The IP address is not associated with spamming.")]
    public bool IsNotSpam { get; }
    
    public IpInfoResults(string ipAddress, X4BNetListsVpnResults vpnListResults, DnsBlResponseType dnsBlResponse)
    {
        IpAddress = ipAddress;
        
        IsVpn = vpnListResults.HasFlag(X4BNetListsVpnResults.Vpn);
        IsDataCenter = vpnListResults.HasFlag(X4BNetListsVpnResults.DataCenter);

        IsBruteForce = dnsBlResponse.HasFlag(DnsBlResponseType.BruteForce);
        IsSpam = dnsBlResponse.HasFlag(DnsBlResponseType.Spam);
        IsBogon = dnsBlResponse.HasFlag(DnsBlResponseType.Bogon);
        IsProxyHttp = dnsBlResponse.HasFlag(DnsBlResponseType.ProxyHttp);
        IsProxySocks = dnsBlResponse.HasFlag(DnsBlResponseType.ProxySocks);
        IsProxyOther = dnsBlResponse.HasFlag(DnsBlResponseType.ProxyOther);
        IsSmtpRelay = dnsBlResponse.HasFlag(DnsBlResponseType.SmtpRelay);
        IsWebVuln = dnsBlResponse.HasFlag(DnsBlResponseType.WebVuln);
        IsNoMail = dnsBlResponse.HasFlag(DnsBlResponseType.NoMail);
        IsZombie = dnsBlResponse.HasFlag(DnsBlResponseType.Zombie);
        IsPotentialZombie = dnsBlResponse.HasFlag(DnsBlResponseType.PotentialZombie);
        IsDynamic = dnsBlResponse.HasFlag(DnsBlResponseType.Dynamic);
        IsNoServer = dnsBlResponse.HasFlag(DnsBlResponseType.NoServer);
        IsBadConf = dnsBlResponse.HasFlag(DnsBlResponseType.BadConf);
        IsProxyWeb = dnsBlResponse.HasFlag(DnsBlResponseType.ProxyWeb);
        IsDDos = dnsBlResponse.HasFlag(DnsBlResponseType.DDos);
        IsOpenDns = dnsBlResponse.HasFlag(DnsBlResponseType.OpenDns);
        IsCompromised = dnsBlResponse.HasFlag(DnsBlResponseType.Compromised);
        IsWorm = dnsBlResponse.HasFlag(DnsBlResponseType.Worm);
        IsIrcDrone = dnsBlResponse.HasFlag(DnsBlResponseType.IrcDrone);
        IsNotSpam = dnsBlResponse.HasFlag(DnsBlResponseType.NoSpam);
    }
}
