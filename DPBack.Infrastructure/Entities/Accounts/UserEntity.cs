using System.ComponentModel.DataAnnotations;
using DPBack.Domain.Enums;

namespace DPBack.Infrastructure.Entities.Accounts;



public class UserEntity
{
    public Guid Id { get; private set; }
    [MaxLength(50)]
    public string Login { get; set; }
    [MaxLength(200)]
    public string PasswordHash { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    
    public UserEntity(Guid id, string login, string passwordHash, string email, UserRole role, DateTime createdAt)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Email = email;
        Role = role;
        CreatedAt = createdAt;
    }
}