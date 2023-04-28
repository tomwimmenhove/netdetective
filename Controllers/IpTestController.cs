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
    private readonly IpTestControllerSettings _settings;
    private readonly IIpQuerier _ipQuerier;
    private readonly IRapidApiRequestInfoProvider _requestInfoProvider;

    public IpTestController(IIpQuerier ipQuerier,
        IOptions<IpTestControllerSettings> settings, IRapidApiRequestInfoProvider requestInfoProvider)
    {
        _ipQuerier = ipQuerier;
        _settings = settings.Value;
        _requestInfoProvider = requestInfoProvider;
    }

    /// <summary>
    /// Query information about an IP address.
    /// </summary>
    /// <param name="ipAddress">
    /// The IP address to use for the query.
    /// Defaults to the ip address of the connecting client.
    /// </param>
    /// <returns>Information about the given IP address</returns>
    [HttpGet("/query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QueryResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SimpleErrorResponeDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(SimpleErrorResponeDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(SimpleErrorResponeDto))]
    public async Task<IActionResult> Query([FromQuery(Name = "ipaddress")] string? ipAddress = null)
    {
        ipAddress ??= _requestInfoProvider.GetClientAddresses().FirstOrDefault()?.ToString();
        if (ipAddress == null)
        {
            return BadRequest(new SimpleErrorResponeDto { Message = "Unable to determine IP address"});            
        }

        var cts = new CancellationTokenSource(_settings.TimeOut);

        try
        {
            var result = await _ipQuerier.Query(ipAddress, cts.Token);
            return Ok(new QueryResult { Result = result});
        }
        catch (FormatException e)
        {
            return BadRequest(new SimpleErrorResponeDto { Message = e.Message});
        }
    }
}
