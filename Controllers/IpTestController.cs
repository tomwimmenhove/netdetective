using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using netdetective.RequestInfoProviders;
using netdetective.IpTests;
using netdetective.Dtos;

namespace netdetective.Controllers;

[ApiController]
[Route("[controller]")]
public class IpTestController : ControllerBase
{
    private readonly ILogger<IpTestController> _logger;
    private readonly IpTestControllerSettings _settings;
    private readonly IIpTestFactory _ipTestFactory;
    private readonly IRapidApiRequestInfoProvider _requestInfoProvider;

    public IpTestController(ILogger<IpTestController> logger, IIpTestFactory ipTestFactory,
        IOptions<IpTestControllerSettings> settings, IRapidApiRequestInfoProvider requestInfoProvider)
    {
        _logger = logger;
        _ipTestFactory = ipTestFactory;
        _settings = settings.Value;
        _requestInfoProvider = requestInfoProvider;
    }

    [HttpGet("/query")]
    [EndpointDescription("Query information about an IP address")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SimpleErrorResponeDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(SimpleErrorResponeDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(SimpleErrorResponeDto))]
    public async Task<IActionResult> Query([FromQuery(Name = "ipaddress")] string? ipAddress = null)
    {
        var clientIp = _requestInfoProvider.GetClientAddresses().FirstOrDefault()?.ToString();

        _logger.LogInformation($"{clientIp} - " +
            $"query: username=\"{_requestInfoProvider.GetUsername()}\", ipaddress=\"{ipAddress}\"");

        ipAddress ??= clientIp;
        if (ipAddress == null)
        {
            return BadRequest(new SimpleErrorResponeDto { Message = "Unable to determine IP address"});            
        }

        var ipTest = await _ipTestFactory.GetInstance();
        var cts = new CancellationTokenSource(_settings.TimeOut);

        try
        {
            var result = await ipTest.Test(ipAddress, cts.Token);
            return Ok(new QueryResult { Result = result});
        }
        catch (FormatException e)
        {
            return BadRequest(new SimpleErrorResponeDto { Message = e.Message});
        }
    }
}
