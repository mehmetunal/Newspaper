using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Yorum güncelleme DTO'su
    /// </summary>
    public class UpdateCommentDto
    {
        /// <summary>
        /// Yorum ID
        /// </summary>
        [Required(ErrorMessage = "Yorum ID zorunludur")]
        public Guid Id { get; set; }

        /// <summary>
        /// Yorum içeriği
        /// </summary>
        [Required(ErrorMessage = "Yorum içeriği zorunludur")]
        [MaxLength(1000, ErrorMessage = "Yorum içeriği en fazla 1000 karakter olabilir")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Onay durumu
        /// </summary>
        public int Status { get; set; } = 0;
    }
} 