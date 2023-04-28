using netdetective.IpTests;

namespace netdetective.Dtos;

public class QueryResult
{
    /// <summary>
    /// Whether or not the call was successful.
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// The result of the query.
    /// </summary>
    public IpInfoResults Result { get; set; } = default!;
}
