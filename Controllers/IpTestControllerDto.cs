using netdetective.IpTests;

namespace netdetective.Controllers;

public class SimpleErrorResponeDto
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = default!;
}

public class QueryResult
{
    public bool Success { get; set; } = true;
    public IpInfoResults Result { get; set; } = default!;
}
