namespace Newspaper.Api.Services
{
    /// <summary>
    /// Seed Data Servisi Interface'i
    /// </summary>
    public interface ISeedDataService
    {
        /// <summary>
        /// Tüm seed data'yı oluştur
        /// </summary>
        Task SeedAllDataAsync();

        /// <summary>
        /// Rolleri oluştur
        /// </summary>
        Task CreateRolesAsync();

        /// <summary>
        /// Admin kullanıcısını oluştur
        /// </summary>
        Task CreateAdminUserAsync();

        /// <summary>
        /// Kategorileri oluştur
        /// </summary>
        Task CreateCategoriesAsync();

        /// <summary>
        /// Tag'leri oluştur
        /// </summary>
        Task CreateTagsAsync();

        /// <summary>
        /// Makaleleri oluştur
        /// </summary>
        Task CreateArticlesAsync();

        /// <summary>
        /// Yorumları oluştur
        /// </summary>
        Task CreateCommentsAsync();

        /// <summary>
        /// Seed data durumunu kontrol et
        /// </summary>
        Task<SeedDataStatus> GetSeedDataStatusAsync();
    }

    /// <summary>
    /// Seed Data Durumu
    /// </summary>
    public class SeedDataStatus
    {
        public bool HasRoles { get; set; }
        public bool HasAdminUser { get; set; }
        public bool HasCategories { get; set; }
        public bool HasTags { get; set; }
        public bool HasArticles { get; set; }
        public bool HasComments { get; set; }
        public int RoleCount { get; set; }
        public int CategoryCount { get; set; }
        public int TagCount { get; set; }
        public int ArticleCount { get; set; }
        public int CommentCount { get; set; }
    }
} 