using System.ComponentModel.DataAnnotations;

namespace netdetective.Auth;

public class ConnectionValidationSettings
{
    [Required]
    public string SecretHeader { get; set; } = default!;

    [Required]
    public string SecretValue { get; set; } = default!;
}
