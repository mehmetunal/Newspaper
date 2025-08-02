using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Etiket detay DTO'su
    /// </summary>
    public class TagDetailDto : BaseDto
    {
        /// <summary>
        /// Etiket adı
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// SEO dostu URL
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Etiket açıklaması
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Aktif durumu
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Kullanım sayısı
        /// </summary>
        public int UsageCount { get; set; } = 0;

        /// <summary>
        /// Makale sayısı
        /// </summary>
        public int ArticleCount { get; set; } = 0;
    }
} 