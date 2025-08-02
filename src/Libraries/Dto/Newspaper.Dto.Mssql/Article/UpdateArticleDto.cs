using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Makale güncelleme DTO'su
    /// </summary>
    public class UpdateArticleDto
    {
        /// <summary>
        /// Makale ID
        /// </summary>
        [Required(ErrorMessage = "Makale ID zorunludur")]
        public Guid Id { get; set; }

        /// <summary>
        /// Makale başlığı
        /// </summary>
        [Required(ErrorMessage = "Makale başlığı zorunludur")]
        [MaxLength(200, ErrorMessage = "Makale başlığı en fazla 200 karakter olabilir")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Makale içeriği
        /// </summary>
        [Required(ErrorMessage = "Makale içeriği zorunludur")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Kısa açıklama
        /// </summary>
        [MaxLength(500, ErrorMessage = "Kısa açıklama en fazla 500 karakter olabilir")]
        public string? Summary { get; set; }

        /// <summary>
        /// SEO dostu URL
        /// </summary>
        [Required(ErrorMessage = "SEO URL zorunludur")]
        [MaxLength(200, ErrorMessage = "SEO URL en fazla 200 karakter olabilir")]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Kapak resmi URL'i
        /// </summary>
        [MaxLength(500, ErrorMessage = "Kapak resmi URL'i en fazla 500 karakter olabilir")]
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Meta keywords
        /// </summary>
        [MaxLength(500, ErrorMessage = "Meta keywords en fazla 500 karakter olabilir")]
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// Meta description
        /// </summary>
        [MaxLength(500, ErrorMessage = "Meta description en fazla 500 karakter olabilir")]
        public string? MetaDescription { get; set; }

        /// <summary>
        /// Kategori ID
        /// </summary>
        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Yayın tarihi
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Öne çıkan makale mi?
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Ana sayfada gösterilsin mi?
        /// </summary>
        public bool IsOnHomePage { get; set; } = false;

        /// <summary>
        /// Yayın durumu
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// Yayınlanmış mı?
        /// </summary>
        public bool IsPublish { get; set; } = false;

        /// <summary>
        /// Okuma süresi (dakika)
        /// </summary>
        public int ReadingTime { get; set; } = 0;

        /// <summary>
        /// Etiket ID'leri
        /// </summary>
        public List<Guid> TagIds { get; set; } = new List<Guid>();
    }
} 