using System.ComponentModel.DataAnnotations;

namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Makale entity'si
    /// </summary>
    public class Article : BaseEntity
    {
        /// <summary>
        /// Makale başlığı
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Makale içeriği
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Kısa açıklama (meta description)
        /// </summary>
        [MaxLength(500)]
        public string? Summary { get; set; }

        /// <summary>
        /// SEO dostu URL
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Kapak resmi URL'i
        /// </summary>
        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Meta keywords
        /// </summary>
        [MaxLength(500)]
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// Meta description
        /// </summary>
        [MaxLength(500)]
        public string? MetaDescription { get; set; }

        /// <summary>
        /// Yazar ID
        /// </summary>
        [Required]
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Kategori ID
        /// </summary>
        [Required]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Yayın tarihi
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Görüntülenme sayısı
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// Beğeni sayısı
        /// </summary>
        public int LikeCount { get; set; } = 0;

        /// <summary>
        /// Yorum sayısı
        /// </summary>
        public int CommentCount { get; set; } = 0;

        /// <summary>
        /// Paylaşım sayısı
        /// </summary>
        public int ShareCount { get; set; } = 0;

        /// <summary>
        /// Öne çıkan makale mi?
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Ana sayfada gösterilsin mi?
        /// </summary>
        public bool IsOnHomePage { get; set; } = false;

        /// <summary>
        /// Yayın durumu (0: Taslak, 1: İncelemede, 2: Yayında, 3: Reddedildi)
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// Okuma süresi (dakika)
        /// </summary>
        public int ReadingTime { get; set; } = 0;

        // Navigation Properties
        public virtual User Author { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
    }
} 