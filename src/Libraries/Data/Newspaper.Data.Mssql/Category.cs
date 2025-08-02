using System.ComponentModel.DataAnnotations;

namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Kategori entity'si
    /// </summary>
    public class Category : BaseEntity
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
        /// Kategori ikonu (CSS class veya icon name)
        /// </summary>
        [MaxLength(50)]
        public string? Icon { get; set; }

        /// <summary>
        /// Renk kodu
        /// </summary>
        [MaxLength(7)]
        public string? Color { get; set; }

        /// <summary>
        /// Üst kategori ID (hierarchical yapı için)
        /// </summary>
        public Guid? ParentCategoryId { get; set; }

        /// <summary>
        /// Sıralama
        /// </summary>
        public int Order { get; set; } = 0;

        // Navigation Properties
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
} 