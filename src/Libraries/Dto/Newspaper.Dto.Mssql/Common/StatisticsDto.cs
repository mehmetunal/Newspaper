namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// İstatistik DTO'su
    /// </summary>
    public class StatisticsDto
    {
        /// <summary>
        /// Toplam kullanıcı sayısı
        /// </summary>
        public int TotalUsers { get; set; } = 0;

        /// <summary>
        /// Toplam makale sayısı
        /// </summary>
        public int TotalArticles { get; set; } = 0;

        /// <summary>
        /// Toplam kategori sayısı
        /// </summary>
        public int TotalCategories { get; set; } = 0;

        /// <summary>
        /// Toplam yorum sayısı
        /// </summary>
        public int TotalComments { get; set; } = 0;

        /// <summary>
        /// Toplam etiket sayısı
        /// </summary>
        public int TotalTags { get; set; } = 0;

        /// <summary>
        /// Bu ay yayınlanan makale sayısı
        /// </summary>
        public int ArticlesThisMonth { get; set; } = 0;

        /// <summary>
        /// Bu hafta yayınlanan makale sayısı
        /// </summary>
        public int ArticlesThisWeek { get; set; } = 0;

        /// <summary>
        /// Bugün yayınlanan makale sayısı
        /// </summary>
        public int ArticlesToday { get; set; } = 0;

        /// <summary>
        /// Toplam görüntülenme sayısı
        /// </summary>
        public int TotalViews { get; set; } = 0;

        /// <summary>
        /// Toplam beğeni sayısı
        /// </summary>
        public int TotalLikes { get; set; } = 0;
    }
} 