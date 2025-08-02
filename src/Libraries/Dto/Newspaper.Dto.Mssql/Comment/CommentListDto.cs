using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Yorum listesi DTO'su
    /// </summary>
    public class CommentListDto : BaseDto
    {
        /// <summary>
        /// Yorum içeriği
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Yazar adı
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;

        /// <summary>
        /// Üst yorum içeriği
        /// </summary>
        public string? ParentCommentContent { get; set; }

        /// <summary>
        /// Makale başlığı
        /// </summary>
        public string ArticleTitle { get; set; } = string.Empty;

        /// <summary>
        /// Kategori adı
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Beğeni sayısı
        /// </summary>
        public int LikeCount { get; set; } = 0;

        /// <summary>
        /// Onay durumu
        /// </summary>
        public string Status { get; set; } = "pending";

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// IP adresi
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User agent
        /// </summary>
        public string? UserAgent { get; set; }
    }
} 