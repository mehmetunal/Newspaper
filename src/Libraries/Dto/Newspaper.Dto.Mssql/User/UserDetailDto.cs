using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Kullanıcı detayı için DTO
    /// </summary>
    public class UserDetailDto : BaseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? Biography { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Gender { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public int ArticleCount { get; set; }
        public int CommentCount { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
} 