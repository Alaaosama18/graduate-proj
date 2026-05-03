using System.ComponentModel.DataAnnotations;

namespace graduate_proj.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // الصلاحية: (Admin أو User)
        public string Role { get; set; } = "User";

        // حالة الحساب: (true نشط، false محظور)
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}