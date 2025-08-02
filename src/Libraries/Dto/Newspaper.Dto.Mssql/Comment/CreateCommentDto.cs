using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Yorum oluşturma DTO'su
    /// </summary>
    public class CreateCommentDto
    {
        /// <summary>
        /// Yorum içeriği
        /// </summary>
        [Required(ErrorMessage = "Yorum içeriği zorunludur")]
        [MaxLength(1000, ErrorMessage = "Yorum içeriği en fazla 1000 karakter olabilir")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Makale ID
        /// </summary>
        [Required(ErrorMessage = "Makale ID zorunludur")]
        public Guid ArticleId { get; set; }

        /// <summary>
        /// Üst yorum ID (reply için)
        /// </summary>
        public Guid? ParentCommentId { get; set; }

        /// <summary>
        /// IP adresi
        /// </summary>
        [MaxLength(45)]
        public string? IpAddress { get; set; }

        /// <summary>
        /// User agent
        /// </summary>
        [MaxLength(500)]
        public string? UserAgent { get; set; }
    }
} 