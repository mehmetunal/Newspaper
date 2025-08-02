using System.ComponentModel.DataAnnotations;

namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Etiket entity'si
    /// </summary>
    public class Tag : BaseEntity
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
        /// Kullanım sayısı
        /// </summary>
        public int UsageCount { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
    }
} 