using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Kategori listesi DTO'su
    /// </summary>
    public class CategoryListDto : BaseDto
    {
        /// <summary>
        /// Kategori adı
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kategori açıklaması
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// SEO dostu URL
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Kategori ikonu
        /// </summary>
        [MaxLength(50)]
        public string? Icon { get; set; }

        /// <summary>
        /// Renk kodu
        /// </summary>
        [MaxLength(7)]
        public string? Color { get; set; }

        /// <summary>
        /// Üst kategori ID
        /// </summary>
        public Guid? ParentCategoryId { get; set; }

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Aktif durumu
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Makale sayısı
        /// </summary>
        public int ArticleCount { get; set; } = 0;
    }
} 