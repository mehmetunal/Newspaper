using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Newspaper.Dto.Mssql;
using Newspaper.Mssql.Services;
using Maggsoft.Core.Model.Pagination;

namespace Newspaper.Api.Controllers
{
    /// <summary>
    /// Yorum işlemleri
    /// </summary>
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(
            ICommentService commentService,
            ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Yorumları listeler
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="articleId">Makale ID</param>
        /// <param name="status">Yorum durumu</param>
        /// <returns>Yorum listesi</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Result<PagedList<CommentListDto>>>> GetComments(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? articleId = null,
            [FromQuery] int? status = null)
        {
            try
            {
                var comments = await _commentService.GetCommentsAsync(page, pageSize, articleId, status);
                return Success(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorumlar listelenirken hata oluştu");
                return Error<PagedList<CommentListDto>>("Yorumlar listelenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Makale yorumlarını getirir
        /// </summary>
        /// <param name="articleId">Makale ID</param>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <returns>Makale yorumları</returns>
        [HttpGet("article/{articleId}")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<PagedList<CommentListDto>>>> GetCommentsByArticle(
            Guid articleId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var comments = await _commentService.GetCommentsByArticleAsync(articleId, page, pageSize);
                return Success(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale yorumları getirilirken hata oluştu. Article ID: {ArticleId}", articleId);
                return Error<PagedList<CommentListDto>>("Makale yorumları getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Son yorumları getirir
        /// </summary>
        /// <param name="count">Yorum sayısı</param>
        /// <returns>Son yorumlar</returns>
        [HttpGet("recent")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<List<CommentListDto>>>> GetRecentComments([FromQuery] int count = 10)
        {
            try
            {
                var comments = await _commentService.GetRecentCommentsAsync(count);
                return Success(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Son yorumlar getirilirken hata oluştu");
                return Error<List<CommentListDto>>("Son yorumlar getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Bekleyen yorumları getirir
        /// </summary>
        /// <param name="count">Yorum sayısı</param>
        /// <returns>Bekleyen yorumlar</returns>
        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<List<CommentListDto>>>> GetPendingComments([FromQuery] int count = 10)
        {
            try
            {
                var comments = await _commentService.GetPendingCommentsAsync(count);
                return Success(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bekleyen yorumlar getirilirken hata oluştu");
                return Error<List<CommentListDto>>("Bekleyen yorumlar getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yorum detayını getirir
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>Yorum detayı</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<CommentListDto>>> GetCommentById(Guid id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    return Error<CommentListDto>("Yorum bulunamadı", 404);
                }

                return Success(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum detayı getirilirken hata oluştu. ID: {CommentId}", id);
                return Error<CommentListDto>("Yorum detayı getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yeni yorum oluşturur
        /// </summary>
        /// <param name="createCommentDto">Yorum oluşturma DTO'su</param>
        /// <returns>Oluşturulan yorum</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Result<CommentListDto>>> CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Error<CommentListDto>("Kullanıcı bilgisi bulunamadı", 401);
                }

                // IP adresi ve User Agent bilgilerini ekle
                createCommentDto.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                createCommentDto.UserAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                var comment = await _commentService.CreateCommentAsync(createCommentDto, userId.Value);
                return Success(comment, "Yorum başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum oluşturulurken hata oluştu");
                return Error<CommentListDto>("Yorum oluşturulurken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yorum günceller
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <param name="updateCommentDto">Yorum güncelleme DTO'su</param>
        /// <returns>Güncellenen yorum</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Result<CommentListDto>>> UpdateComment(Guid id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            try
            {
                updateCommentDto.Id = id;
                var comment = await _commentService.UpdateCommentAsync(updateCommentDto);
                return Success(comment, "Yorum başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum güncellenirken hata oluştu. ID: {CommentId}", id);
                return Error<CommentListDto>("Yorum güncellenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yorum siler
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Result<object>>> DeleteComment(Guid id)
        {
            try
            {
                var result = await _commentService.DeleteCommentAsync(id);
                if (!result)
                {
                    return Error<object>("Yorum silinemedi", 400);
                }

                return Success<object>(null, "Yorum başarıyla silindi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum silinirken hata oluştu. ID: {CommentId}", id);
                return Error<object>("Yorum silinirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yorumu geri yükler
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/restore")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<object>>> RestoreComment(Guid id)
        {
            try
            {
                var result = await _commentService.RestoreCommentAsync(id);
                if (!result)
                {
                    return Error<object>("Yorum geri yüklenemedi", 400);
                }

                return Success<object>(null, "Yorum başarıyla geri yüklendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum geri yüklenirken hata oluştu. ID: {CommentId}", id);
                return Error<object>("Yorum geri yüklenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yorum durumunu günceller
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <param name="status">Yeni durum</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<object>>> UpdateCommentStatus(Guid id, [FromBody] int status)
        {
            try
            {
                var result = await _commentService.UpdateCommentStatusAsync(id, status);
                if (!result)
                {
                    return Error<object>("Yorum durumu güncellenemedi", 400);
                }

                return Success<object>(null, "Yorum durumu başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum durumu güncellenirken hata oluştu. ID: {CommentId}", id);
                return Error<object>("Yorum durumu güncellenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yorum beğeni sayısını artırır
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/like")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<object>>> LikeComment(Guid id)
        {
            try
            {
                var result = await _commentService.IncrementLikeCountAsync(id);
                if (!result)
                {
                    return Error<object>("Yorum beğenilemedi", 400);
                }

                return Success<object>(null, "Yorum beğenildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum beğenilirken hata oluştu. ID: {CommentId}", id);
                return Error<object>("Yorum beğenilirken hata oluştu", 500);
            }
        }
    }
} 