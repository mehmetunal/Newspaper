using Newspaper.Dto.Mssql;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.IoC;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Makale servis interface'i
    /// </summary>
    public interface IArticleService : IService
    {
        /// <summary>
        /// Makale listesini getirir
        /// </summary>
        /// <param name="searchDto">Arama parametreleri</param>
        /// <returns>Sayfalanmış makale listesi</returns>
        Task<PagedList<ArticleListDto>> GetArticlesAsync(ArticleSearchDto searchDto);

        /// <summary>
        /// Makale detayını getirir
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>Makale detayı</returns>
        Task<ArticleDetailDto?> GetArticleByIdAsync(Guid id);

        /// <summary>
        /// Makale detayını slug ile getirir
        /// </summary>
        /// <param name="slug">Makale slug</param>
        /// <returns>Makale detayı</returns>
        Task<ArticleDetailDto?> GetArticleBySlugAsync(string slug);

        /// <summary>
        /// Makale oluşturur
        /// </summary>
        /// <param name="createArticleDto">Makale oluşturma DTO'su</param>
        /// <returns>Oluşturulan makale</returns>
        Task<ArticleDetailDto> CreateArticleAsync(CreateArticleDto createArticleDto);

        /// <summary>
        /// Makale günceller
        /// </summary>
        /// <param name="updateArticleDto">Makale güncelleme DTO'su</param>
        /// <returns>Güncellenen makale</returns>
        Task<ArticleDetailDto> UpdateArticleAsync(UpdateArticleDto updateArticleDto);

        /// <summary>
        /// Makale siler (soft delete)
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> DeleteArticleAsync(Guid id);

        /// <summary>
        /// Makaleyi geri yükler
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> RestoreArticleAsync(Guid id);

        /// <summary>
        /// Öne çıkan makaleleri getirir
        /// </summary>
        /// <param name="count">Makale sayısı</param>
        /// <returns>Öne çıkan makaleler</returns>
        Task<List<ArticleListDto>> GetFeaturedArticlesAsync(int count = 5);

        /// <summary>
        /// Ana sayfa makalelerini getirir
        /// </summary>
        /// <param name="count">Makale sayısı</param>
        /// <returns>Ana sayfa makaleleri</returns>
        Task<List<ArticleListDto>> GetHomePageArticlesAsync(int count = 10);

        /// <summary>
        /// Kategori makalelerini getirir
        /// </summary>
        /// <param name="categoryId">Kategori ID</param>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <returns>Kategori makaleleri</returns>
        Task<PagedList<ArticleListDto>> GetArticlesByCategoryAsync(Guid categoryId, int page = 1, int pageSize = 10);

        /// <summary>
        /// Makale görüntülenme sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> IncrementViewCountAsync(Guid id);

        /// <summary>
        /// Makale beğeni sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> IncrementLikeCountAsync(Guid id);

        /// <summary>
        /// Makale paylaşım sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> IncrementShareCountAsync(Guid id);
    }
} 