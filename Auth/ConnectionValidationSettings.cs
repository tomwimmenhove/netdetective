using System.ComponentModel.DataAnnotations;

namespace netdetective.Auth;

public class ConnectionValidationSettings
{
    public string SecretValue { get; set; } = default!;
}
