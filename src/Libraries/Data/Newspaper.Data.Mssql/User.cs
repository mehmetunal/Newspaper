using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Kullanıcı entity'si
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Ad
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Soyad
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Tam ad (hesaplanmış alan)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// Profil resmi URL'i
        /// </summary>
        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }

        /// <summary>
        /// Biyografi
        /// </summary>
        [MaxLength(1000)]
        public string? Biography { get; set; }

        /// <summary>
        /// Doğum tarihi
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Cinsiyet (0: Belirtilmemiş, 1: Erkek, 2: Kadın)
        /// </summary>
        public int Gender { get; set; } = 0;

        /// <summary>
        /// Son giriş tarihi
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Kullanıcının aktif olup olmadığı
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
} 