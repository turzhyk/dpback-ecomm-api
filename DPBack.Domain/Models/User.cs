using DPBack.Domain.Enums;

namespace DPBack.Domain.Models
{
   

    public class User
    {
        public Guid Id { get; set; }
        public required string Login { get; set; }
        public required string PasswordHash { get; set; } = "";
        public required string Email { get; set; }
        public required UserRole Role { get; set; }
        public  required DateTime CreatedAt { get; set; }
     

        public User()
        {
            
        }
        public User(Guid id, string login, string passwordHash, string email, UserRole role, DateTime createdAt)
        {
            Id = id;
            Login = login;
            PasswordHash = passwordHash;
            Email = email;
            Role = role;
            CreatedAt = createdAt;
        }

        public void SetPassword(string password)
        {
            PasswordHash = password;
        }
    }
}