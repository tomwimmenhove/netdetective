using System.ComponentModel;

namespace netdetective.Dtos;

public class SimpleErrorResponeDto
{
    [Description("Wether or not the call was successful.")]
    public bool Success { get; set; } = false;

    [Description("An error message.")]
    public string Message { get; set; } = default!;
}
