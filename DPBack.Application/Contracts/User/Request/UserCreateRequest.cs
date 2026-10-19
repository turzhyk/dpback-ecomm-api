using System.ComponentModel.DataAnnotations;

namespace DPBack.Application.Contracts.User.Request;

public class UserCreateRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required ]
    public required string Password { get; set; }
    public string? Name { get; set; }
    [Phone]
    public string? Phone { get; set; }
}