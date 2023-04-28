using System.ComponentModel.DataAnnotations;

namespace netdetective.IpTests;

public class IpInfoSettings
{
    [Required]
    public string VpnListPath { get; set; } = default!;

    [Required]
    public string DatacenterListPath { get; set; } = default!;

    [Required]
    public string DnsBlDatabasePath { get; set; } = default!;
}
