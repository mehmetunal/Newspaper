using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Yorum detay DTO'su
    /// </summary>
    public class CommentDetailDto : BaseDto
    {
        /// <summary>
        /// Yorum içeriği
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Yorum sahibi kullanıcı ID
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Yorum sahibi kullanıcı adı
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Yazar adı
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;

        /// <summary>
        /// Yorum sahibi kullanıcı email
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Yazar email
        /// </summary>
        public string AuthorEmail { get; set; } = string.Empty;

        /// <summary>
        /// Üst yorum içeriği
        /// </summary>
        public string? ParentCommentContent { get; set; }

        /// <summary>
        /// Haber ID
        /// </summary>
        [Required]
        public string ArticleId { get; set; } = string.Empty;

        /// <summary>
        /// Haber başlığı
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ArticleTitle { get; set; } = string.Empty;

        /// <summary>
        /// Kategori adı
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Makale yazar adı
        /// </summary>
        public string ArticleAuthorName { get; set; } = string.Empty;

        /// <summary>
        /// Makale yayın tarihi
        /// </summary>
        public DateTime? ArticlePublishedAt { get; set; }

        /// <summary>
        /// Yorum durumu
        /// </summary>
        public string Status { get; set; } = "pending";

        /// <summary>
        /// Yorum onay durumu
        /// </summary>
        public bool IsApproved { get; set; } = false;

        /// <summary>
        /// Yorum aktif durumu
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Güncellenme tarihi
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Onaylanma tarihi
        /// </summary>
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// Reddedilme tarihi
        /// </summary>
        public DateTime? RejectedAt { get; set; }

        /// <summary>
        /// IP adresi
        /// </summary>
        [MaxLength(45)]
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı agent bilgisi
        /// </summary>
        [MaxLength(500)]
        public string UserAgent { get; set; } = string.Empty;
    }
} 