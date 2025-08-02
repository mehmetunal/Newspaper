using Newspaper.Dto.Mssql;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.IoC;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Yorum servis interface'i
    /// </summary>
    public interface ICommentService : IService
    {
        /// <summary>
        /// Yorum listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="articleId">Makale ID (opsiyonel)</param>
        /// <param name="status">Onay durumu (opsiyonel)</param>
        /// <returns>Sayfalanmış yorum listesi</returns>
        Task<PagedList<CommentListDto>> GetCommentsAsync(int page = 1, int pageSize = 10, Guid? articleId = null, int? status = null);

        /// <summary>
        /// Makale yorumlarını getirir
        /// </summary>
        /// <param name="articleId">Makale ID</param>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <returns>Makale yorumları</returns>
        Task<PagedList<CommentListDto>> GetCommentsByArticleAsync(Guid articleId, int page = 1, int pageSize = 10);

        /// <summary>
        /// Yorum oluşturur
        /// </summary>
        /// <param name="createCommentDto">Yorum oluşturma DTO'su</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Oluşturulan yorum</returns>
        Task<CommentListDto> CreateCommentAsync(CreateCommentDto createCommentDto, Guid userId);

        /// <summary>
        /// Yorum günceller
        /// </summary>
        /// <param name="updateCommentDto">Yorum güncelleme DTO'su</param>
        /// <returns>Güncellenen yorum</returns>
        Task<CommentListDto> UpdateCommentAsync(UpdateCommentDto updateCommentDto);

        /// <summary>
        /// Yorum siler (soft delete)
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> DeleteCommentAsync(Guid id);

        /// <summary>
        /// Yorumu geri yükler
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> RestoreCommentAsync(Guid id);

        /// <summary>
        /// Yorum onay durumunu günceller
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <param name="status">Onay durumu</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> UpdateCommentStatusAsync(Guid id, int status);

        /// <summary>
        /// Yorum beğeni sayısını artırır
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> IncrementLikeCountAsync(Guid id);

        /// <summary>
        /// Son yorumları getirir
        /// </summary>
        /// <param name="count">Yorum sayısı</param>
        /// <returns>Son yorumlar</returns>
        Task<List<CommentListDto>> GetRecentCommentsAsync(int count = 10);

        /// <summary>
        /// Bekleyen yorumları getirir
        /// </summary>
        /// <param name="count">Yorum sayısı</param>
        /// <returns>Bekleyen yorumlar</returns>
        Task<List<CommentListDto>> GetPendingCommentsAsync(int count = 10);

        /// <summary>
        /// Yorum detayını getirir
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>Yorum detayı</returns>
        Task<CommentListDto?> GetCommentByIdAsync(Guid id);
    }
} 