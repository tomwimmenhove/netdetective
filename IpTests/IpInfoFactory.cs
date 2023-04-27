using Microsoft.Extensions.Options;

namespace netdetective.IpTests;

public class IpInfoFactory : IIpTestFactory
{
    private readonly IOptions<IpInfoSettings> _settings;
    private IpInfo? _instance;

    public IpInfoFactory(IOptions<IpInfoSettings> settings)
    {
        _settings = settings;
    }

    public async Task<IIpTest> GetInstance() => _instance ??= await IpInfo.Create(_settings);
}
