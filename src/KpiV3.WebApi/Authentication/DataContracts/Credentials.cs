using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.Authentication.DataContracts;

public readonly record struct Credentials
{
    [Required]
    public string Email { get; init; }
    [Required]
    public string Password { get; init; }
}
