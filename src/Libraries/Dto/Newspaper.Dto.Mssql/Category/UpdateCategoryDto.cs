using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Kategori güncelleme DTO'su
    /// </summary>
    public class UpdateCategoryDto
    {
        /// <summary>
        /// Kategori ID
        /// </summary>
        [Required(ErrorMessage = "Kategori ID zorunludur")]
        public Guid Id { get; set; }

        /// <summary>
        /// Kategori adı
        /// </summary>
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [MaxLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kategori açıklaması
        /// </summary>
        [MaxLength(500, ErrorMessage = "Kategori açıklaması en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        /// <summary>
        /// SEO dostu URL
        /// </summary>
        [Required(ErrorMessage = "SEO URL zorunludur")]
        [MaxLength(100, ErrorMessage = "SEO URL en fazla 100 karakter olabilir")]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Kategori ikonu
        /// </summary>
        [MaxLength(50, ErrorMessage = "İkon en fazla 50 karakter olabilir")]
        public string? Icon { get; set; }

        /// <summary>
        /// Renk kodu
        /// </summary>
        [MaxLength(7, ErrorMessage = "Renk kodu en fazla 7 karakter olabilir")]
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
        /// Aktif/Pasif durumu
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 