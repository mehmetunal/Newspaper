using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Kullanıcı güncelleme için DTO
    /// </summary>
    public class UpdateUserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }

        [MaxLength(1000)]
        public string? Biography { get; set; }

        public DateTime? BirthDate { get; set; }
        public int Gender { get; set; } = 0;
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> Roles { get; set; } = new List<string>();
    }
} 