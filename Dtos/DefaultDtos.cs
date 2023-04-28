namespace netdetective.Dtos;

public class SimpleErrorResponeDto
{
    /// <summary>
    /// Whether or not the call was successful.
    /// </summary>
    public bool Success { get; set; } = false;

    /// <summary>
    /// An error message.
    /// </summary>
    public string Message { get; set; } = default!;
}
