using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Makale listesi DTO'su
    /// </summary>
    public class ArticleListDto : BaseDto
    {
        /// <summary>
        /// Makale başlığı
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Kısa açıklama
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
        /// Yazar adı
        /// </summary>
        public string AuthorName { get; set; } = string.Empty;

        /// <summary>
        /// Kategori adı
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Etiketler
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Yayın tarihi
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Yayınlanmış mı?
        /// </summary>
        public bool IsPublished { get; set; } = false;

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
        /// Öne çıkan makale mi?
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Okuma süresi (dakika)
        /// </summary>
        public int ReadingTime { get; set; } = 0;
    }
} 