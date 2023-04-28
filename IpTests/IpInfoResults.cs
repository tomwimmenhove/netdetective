using netdetective.BlackList;

namespace netdetective.IpTests;

public class IpInfoResults
{
    /// <summary>
    /// The IP address being queried.
    /// </summary>
    public string IpAddress {get; }

    /// <summary>
    /// The IP address is a VPN.
    /// </summary>
    public bool IsVpn { get; }
    
    /// <summary>
    /// Indicates whether the IP address belongs to a data center.
    /// </summary>
    public bool IsDataCenter { get; }

    /// <summary>
    /// The IP address is known for brute-force attacks.
    /// </summary>
    public bool IsBruteForce { get; }
    
    /// <summary>
    /// The IP address is known for sending spam.
    /// </summary>
    public bool IsSpam { get; }
    
    /// <summary>
    /// The IP address is a bogon (i.e., it should not appear in the public Internet routing table).
    /// </summary>
    public bool IsBogon { get; }
    
    /// <summary>
    /// The IP address is an open HTTP proxy.
    /// </summary>
    public bool IsProxyHttp { get; }
    
    /// <summary>
    /// The IP address is an open SOCKS proxy.
    /// </summary>
    public bool IsProxySocks { get; }

    /// <summary>
    /// The IP address is a web proxy.
    /// </summary>
    public bool IsProxyWeb { get; }

    /// <summary>
    /// The IP address is a type of proxy.
    /// </summary>
    public bool IsProxyOther { get; }
    
    /// <summary>
    /// The IP address is used to relay SMTP traffic.
    /// </summary>
    public bool IsSmtpRelay { get; }
    
    /// <summary>
    /// The IP address is vulnerable webserver that can be used for abuse.
    /// </summary>
    public bool IsWebVuln { get; }
    
    /// <summary>
    /// The IP address should not be used for email.
    /// </summary>
    public bool IsNoMail { get; }
    
    /// <summary>
    /// The IP address is a zombie (i.e., part of a botnet).
    /// </summary>
    public bool IsZombie { get; }

    /// <summary>
    /// The IP address is a potential zombie.
    /// </summary>
    public bool IsPotentialZombie { get; }
    
    /// <summary>
    /// The IP address is dynamically assigned.
    /// </summary>
    public bool IsDynamic { get; }
    
    /// <summary>
    /// The IP address is not associated with a server.
    /// </summary>
    public bool IsNoServer { get; }
    
    /// <summary>
    /// The IP address is a misconfigured server.
    /// </summary>
    public bool IsBadConf { get; }
        
    /// <summary>
    /// The IP address is known for DDoS attacks.
    /// </summary>
    public bool IsDDos { get; }
    
    /// <summary>
    /// The IP address belongs to an OpenDNS resolver.
    /// </summary>
    public bool IsOpenDns { get; }
    
    /// <summary>
    /// The IP address has been compromised.
    /// </summary>
    public bool IsCompromised { get; }
    
    /// <summary>
    /// The IP address is part of a worm infection.
    /// </summary>
    public bool IsWorm { get; }
        
    /// <summary>
    /// The IP address is an IRC drone
    /// </summary>
    public bool IsIrcDrone { get; }
    
    /// <summary>
    /// The IP address is not associated with spamming.
    /// </summary>
    public bool IsNotSpam { get; }
    
    public IpInfoResults(string ipAddress, X4BNetListsFlags vpnListResults, DnsBlFlags dnsBlResponse)
    {
        IpAddress = ipAddress;
        
        IsVpn = vpnListResults.HasFlag(X4BNetListsFlags.Vpn);
        IsDataCenter = vpnListResults.HasFlag(X4BNetListsFlags.DataCenter);

        IsBruteForce = dnsBlResponse.HasFlag(DnsBlFlags.BruteForce);
        IsSpam = dnsBlResponse.HasFlag(DnsBlFlags.Spam);
        IsBogon = dnsBlResponse.HasFlag(DnsBlFlags.Bogon);
        IsProxyHttp = dnsBlResponse.HasFlag(DnsBlFlags.ProxyHttp);
        IsProxySocks = dnsBlResponse.HasFlag(DnsBlFlags.ProxySocks);
        IsProxyOther = dnsBlResponse.HasFlag(DnsBlFlags.ProxyOther);
        IsSmtpRelay = dnsBlResponse.HasFlag(DnsBlFlags.SmtpRelay);
        IsWebVuln = dnsBlResponse.HasFlag(DnsBlFlags.WebVuln);
        IsNoMail = dnsBlResponse.HasFlag(DnsBlFlags.NoMail);
        IsZombie = dnsBlResponse.HasFlag(DnsBlFlags.Zombie);
        IsPotentialZombie = dnsBlResponse.HasFlag(DnsBlFlags.PotentialZombie);
        IsDynamic = dnsBlResponse.HasFlag(DnsBlFlags.Dynamic);
        IsNoServer = dnsBlResponse.HasFlag(DnsBlFlags.NoServer);
        IsBadConf = dnsBlResponse.HasFlag(DnsBlFlags.BadConf);
        IsProxyWeb = dnsBlResponse.HasFlag(DnsBlFlags.ProxyWeb);
        IsDDos = dnsBlResponse.HasFlag(DnsBlFlags.DDos);
        IsOpenDns = dnsBlResponse.HasFlag(DnsBlFlags.OpenDns);
        IsCompromised = dnsBlResponse.HasFlag(DnsBlFlags.Compromised);
        IsWorm = dnsBlResponse.HasFlag(DnsBlFlags.Worm);
        IsIrcDrone = dnsBlResponse.HasFlag(DnsBlFlags.IrcDrone);
        IsNotSpam = dnsBlResponse.HasFlag(DnsBlFlags.NoSpam);
    }
}
