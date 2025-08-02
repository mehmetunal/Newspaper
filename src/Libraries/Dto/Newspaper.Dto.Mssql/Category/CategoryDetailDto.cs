using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Kategori detay DTO'su
    /// </summary>
    public class CategoryDetailDto : BaseDto
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
        public string? Description { get; set; }

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
        /// Sıralama
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Üst kategori adı
        /// </summary>
        public string? ParentCategoryName { get; set; }

        /// <summary>
        /// Alt kategori sayısı
        /// </summary>
        public int SubCategoryCount { get; set; } = 0;

        /// <summary>
        /// Makale sayısı
        /// </summary>
        public int ArticleCount { get; set; } = 0;

        /// <summary>
        /// Aktif durumu
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 