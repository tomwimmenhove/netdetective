using netdetective.IpTests;
using System.ComponentModel;

namespace netdetective.Dtos;

public class QueryResult
{
    [Description("Wether or not the call was successful.")]
    public bool Success { get; set; } = true;

    [Description("The result of the query.")]
    public IpInfoResults Result { get; set; } = default!;
}
