using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using netdetective.IpTests;

namespace netdetective.Controllers;

[ApiController]
[Route("[controller]")]
public class IpTestController : ControllerBase
{
    private readonly ILogger<IpTestController> _logger;
    private readonly IpTestControllerSettings _settings;
    private readonly IIpTestFactory _ipTestFactory;

    private string GetClientIp() => Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
        HttpContext.Connection.RemoteIpAddress?.ToString() ??
        "unknown";

    private string GetFirstClientIp() => GetClientIp().Split(",").First().Trim();

    public IpTestController(ILogger<IpTestController> logger, IIpTestFactory ipTestFactory,
        IOptions<IpTestControllerSettings> settings)
    {
        _logger = logger;
        _ipTestFactory = ipTestFactory;
        _settings = settings.Value;
    }

    [HttpGet("/query")]
    [EndpointDescription("Query information about an IP address")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SimpleErrorResponeDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(SimpleErrorResponeDto))]
    public async Task<IActionResult> Query([FromQuery(Name = "ipaddress")] string? ipAddress = null)
    {
        _logger.LogInformation($"{GetClientIp()} - " +
            $"queAry: username=\"{ipAddress}\"");

        ipAddress ??= GetFirstClientIp();
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
