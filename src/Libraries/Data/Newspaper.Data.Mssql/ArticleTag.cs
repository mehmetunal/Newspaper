using System.ComponentModel.DataAnnotations;

namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Makale-Etiket ilişki entity'si (Many-to-Many)
    /// </summary>
    public class ArticleTag : BaseEntity
    {
        /// <summary>
        /// Makale ID
        /// </summary>
        [Required]
        public Guid ArticleId { get; set; }

        /// <summary>
        /// Etiket ID
        /// </summary>
        [Required]
        public Guid TagId { get; set; }

        // Navigation Properties
        public virtual Article Article { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
} 