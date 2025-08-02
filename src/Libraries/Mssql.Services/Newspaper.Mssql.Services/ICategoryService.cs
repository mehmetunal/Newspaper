using Newspaper.Dto.Mssql;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.IoC;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Kategori servis interface'i
    /// </summary>
    public interface ICategoryService : IService
    {
        /// <summary>
        /// Kategori listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <param name="parentId">Üst kategori ID</param>
        /// <returns>Sayfalanmış kategori listesi</returns>
        Task<PagedList<CategoryListDto>> GetCategoriesAsync(int page = 1, int pageSize = 10, string? searchTerm = null, Guid? parentId = null);

        /// <summary>
        /// Tüm kategorileri getirir (hiyerarşik)
        /// </summary>
        /// <returns>Kategori listesi</returns>
        Task<List<CategoryListDto>> GetAllCategoriesAsync();

        /// <summary>
        /// Kategori detayını getirir
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>Kategori detayı</returns>
        Task<CategoryDetailDto?> GetCategoryByIdAsync(Guid id);

        /// <summary>
        /// Kategori oluşturur
        /// </summary>
        /// <param name="createCategoryDto">Kategori oluşturma DTO'su</param>
        /// <returns>Oluşturulan kategori</returns>
        Task<CategoryDetailDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);

        /// <summary>
        /// Kategori günceller
        /// </summary>
        /// <param name="updateCategoryDto">Kategori güncelleme DTO'su</param>
        /// <returns>Güncellenen kategori</returns>
        Task<CategoryDetailDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);

        /// <summary>
        /// Kategori siler (soft delete)
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> DeleteCategoryAsync(Guid id);

        /// <summary>
        /// Kategoriyi geri yükler
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> RestoreCategoryAsync(Guid id);

        /// <summary>
        /// Alt kategorileri getirir
        /// </summary>
        /// <param name="parentId">Üst kategori ID</param>
        /// <returns>Alt kategori listesi</returns>
        Task<List<CategoryListDto>> GetSubCategoriesAsync(Guid parentId);
    }
} 