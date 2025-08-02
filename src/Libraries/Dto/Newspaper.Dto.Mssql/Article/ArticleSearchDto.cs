using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Makale arama DTO'su
    /// </summary>
    public class ArticleSearchDto
    {
        /// <summary>
        /// Arama terimi
        /// </summary>
        [MaxLength(200, ErrorMessage = "Arama terimi en fazla 200 karakter olabilir")]
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Kategori ID
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Yazar ID
        /// </summary>
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// Yayın durumu
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Yayınlanmış mı?
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// Öne çıkan makale mi?
        /// </summary>
        public bool? IsFeatured { get; set; }

        /// <summary>
        /// Başlangıç tarihi
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Bitiş tarihi
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Sayfa numarası
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Sayfa numarası 1'den büyük olmalıdır")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Sayfa boyutu
        /// </summary>
        [Range(1, 100, ErrorMessage = "Sayfa boyutu 1-100 arasında olmalıdır")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Sıralama alanı
        /// </summary>
        public string? SortBy { get; set; } = "CreatedDate";

        /// <summary>
        /// Sıralama yönü (asc/desc)
        /// </summary>
        public string? SortDirection { get; set; } = "desc";
    }
} 