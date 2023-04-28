using Microsoft.Extensions.Options;

namespace netdetective.IpTests;

public class IpInfoFactory : IIpTestFactory
{
    private readonly ILogger _logger;
    private readonly IOptions<IpInfoSettings> _settings;
    private IpInfo? _instance;

    public IpInfoFactory(ILogger<IpInfoFactory> logger, IOptions<IpInfoSettings> settings)
    {
        _logger = logger;
        _settings = settings;
    }

    public async Task<IIpTest> GetInstance() => _instance ??= await IpInfo.Create(_logger, _settings);
}
