using System;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Models
{
    public enum UserRole
    {
        ADMINISTRATOR,
        USER
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.USER;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
