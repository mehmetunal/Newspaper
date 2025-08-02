using System.Text.Json.Serialization;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Admin Panel tarafında API'den gelen PagedList'i karşılayacak wrapper sınıfı
    /// </summary>
    public class PagedListWrapper<T>
    {
        /// <summary>
        /// Veri listesi
        /// </summary>
        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new();

        /// <summary>
        /// Sayfa numarası (0-based)
        /// </summary>
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Sayfa boyutu
        /// </summary>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Toplam kayıt sayısı
        /// </summary>
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Toplam sayfa sayısı
        /// </summary>
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Önceki sayfa var mı?
        /// </summary>
        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Sonraki sayfa var mı?
        /// </summary>
        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }

        /// <summary>
        /// 1-based sayfa numarası
        /// </summary>
        public int CurrentPage => PageNumber + 1;

        /// <summary>
        /// Boş PagedList oluşturur
        /// </summary>
        public static PagedListWrapper<T> Empty(int pageNumber = 1, int pageSize = 10)
        {
            return new PagedListWrapper<T>
            {
                Data = new List<T>(),
                PageNumber = pageNumber - 1, // 1-based'den 0-based'e çevir
                PageSize = pageSize,
                TotalCount = 0,
                TotalPages = 0,
                HasPreviousPage = false,
                HasNextPage = false
            };
        }
    }
} 