using System.ComponentModel.DataAnnotations;

namespace TipsRundan.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; }

        // Konstruktor som sätter alla egenskaper
        private User(Guid id, string username, string email, string passwordHash)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }

        public static User Create(string username, string email, string passwordHash)
        {
            return new User(
                Guid.NewGuid(),
                username,
                email,
                passwordHash
            );
        }

        public static User Load(Guid id, string username, string email, string passwordHash)
        {
            return new User(
                id,
                username,
                email,
                passwordHash
            );
        }
    }
}
