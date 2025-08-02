using Newspaper.Dto.Mssql;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.IoC;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Etiket servis interface'i
    /// </summary>
    public interface ITagService : IService
    {
        /// <summary>
        /// Etiket listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sayfalanmış etiket listesi</returns>
        Task<PagedList<TagListDto>> GetTagsAsync(int page = 1, int pageSize = 10, string? searchTerm = null);

        /// <summary>
        /// Tüm etiketleri getirir
        /// </summary>
        /// <returns>Etiket listesi</returns>
        Task<List<TagListDto>> GetAllTagsAsync();

        /// <summary>
        /// Etiket detayını getirir
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>Etiket detayı</returns>
        Task<TagDetailDto?> GetTagByIdAsync(Guid id);

        /// <summary>
        /// Etiket oluşturur
        /// </summary>
        /// <param name="createTagDto">Etiket oluşturma DTO'su</param>
        /// <returns>Oluşturulan etiket</returns>
        Task<TagDetailDto> CreateTagAsync(CreateTagDto createTagDto);

        /// <summary>
        /// Etiket günceller
        /// </summary>
        /// <param name="updateTagDto">Etiket güncelleme DTO'su</param>
        /// <returns>Güncellenen etiket</returns>
        Task<TagDetailDto> UpdateTagAsync(UpdateTagDto updateTagDto);

        /// <summary>
        /// Etiket siler (soft delete)
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> DeleteTagAsync(Guid id);

        /// <summary>
        /// Etiketi geri yükler
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> RestoreTagAsync(Guid id);

        /// <summary>
        /// Popüler etiketleri getirir
        /// </summary>
        /// <param name="count">Etiket sayısı</param>
        /// <returns>Popüler etiketler</returns>
        Task<List<TagListDto>> GetPopularTagsAsync(int count = 10);

        /// <summary>
        /// Etiket kullanım sayısını artırır
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> IncrementUsageCountAsync(Guid id);
    }
} 