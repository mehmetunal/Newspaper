using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Şifre değiştirme için DTO
    /// </summary>
    public class ChangePasswordDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
} 