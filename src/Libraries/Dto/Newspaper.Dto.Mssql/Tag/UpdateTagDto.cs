using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Etiket güncelleme DTO'su
    /// </summary>
    public class UpdateTagDto
    {
        /// <summary>
        /// Etiket ID
        /// </summary>
        [Required(ErrorMessage = "Etiket ID zorunludur")]
        public Guid Id { get; set; }

        /// <summary>
        /// Etiket adı
        /// </summary>
        [Required(ErrorMessage = "Etiket adı zorunludur")]
        [MaxLength(50, ErrorMessage = "Etiket adı en fazla 50 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// SEO dostu URL
        /// </summary>
        [Required(ErrorMessage = "SEO URL zorunludur")]
        [MaxLength(50, ErrorMessage = "SEO URL en fazla 50 karakter olabilir")]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Etiket açıklaması
        /// </summary>
        [MaxLength(200, ErrorMessage = "Etiket açıklaması en fazla 200 karakter olabilir")]
        public string? Description { get; set; }
    }
} 