using System.ComponentModel.DataAnnotations;

namespace DPBack.Infrastructure.Entities.Accounts;

public class RefreshTokenEntity
{
    
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    
    [MaxLength(500)]
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
}