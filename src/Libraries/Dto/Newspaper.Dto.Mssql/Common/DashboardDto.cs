namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Dashboard DTO'su
    /// </summary>
    public class DashboardDto
    {
        /// <summary>
        /// İstatistikler
        /// </summary>
        public StatisticsDto Statistics { get; set; } = new StatisticsDto();

        /// <summary>
        /// Son makaleler
        /// </summary>
        public List<ArticleListDto> RecentArticles { get; set; } = new List<ArticleListDto>();

        /// <summary>
        /// Popüler makaleler
        /// </summary>
        public List<ArticleListDto> PopularArticles { get; set; } = new List<ArticleListDto>();

        /// <summary>
        /// Son kullanıcılar
        /// </summary>
        public List<UserListDto> RecentUsers { get; set; } = new List<UserListDto>();

        /// <summary>
        /// Son yorumlar
        /// </summary>
        public List<CommentListDto> RecentComments { get; set; } = new List<CommentListDto>();

        /// <summary>
        /// Kategori dağılımı
        /// </summary>
        public List<CategoryStatisticsDto> CategoryDistribution { get; set; } = new List<CategoryStatisticsDto>();
    }



    /// <summary>
    /// Kategori istatistik DTO'su
    /// </summary>
    public class CategoryStatisticsDto
    {
        /// <summary>
        /// Kategori adı
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Makale sayısı
        /// </summary>
        public int ArticleCount { get; set; } = 0;

        /// <summary>
        /// Yüzde
        /// </summary>
        public double Percentage { get; set; } = 0;
    }
} 