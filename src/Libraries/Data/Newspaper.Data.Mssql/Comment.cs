using System.ComponentModel.DataAnnotations;

namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Yorum entity'si
    /// </summary>
    public class Comment : BaseEntity
    {
        /// <summary>
        /// Yorum içeriği
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Makale ID
        /// </summary>
        [Required]
        public Guid ArticleId { get; set; }

        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Üst yorum ID (reply için)
        /// </summary>
        public Guid? ParentCommentId { get; set; }

        /// <summary>
        /// Beğeni sayısı
        /// </summary>
        public int LikeCount { get; set; } = 0;

        /// <summary>
        /// Onay durumu (0: Beklemede, 1: Onaylandı, 2: Reddedildi)
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// IP adresi
        /// </summary>
        [MaxLength(45)]
        public string? IpAddress { get; set; }

        /// <summary>
        /// User agent
        /// </summary>
        [MaxLength(500)]
        public string? UserAgent { get; set; }

        // Navigation Properties
        public virtual Article Article { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
} 