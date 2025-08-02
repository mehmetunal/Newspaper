using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newspaper.Data.Mssql;
using Newspaper.Dto.Mssql;
using Maggsoft.Mssql.Repository;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.Model;
using Maggsoft.Core.Extensions;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Yorum servis implementasyonu
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly IMssqlRepository<Comment> _commentRepository;
        private readonly IMssqlRepository<Article> _articleRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CommentService> _logger;

        public CommentService(
            IMssqlRepository<Comment> commentRepository,
            IMssqlRepository<Article> articleRepository,
            UserManager<User> userManager,
            ILogger<CommentService> logger)
        {
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Yorum listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="articleId">Makale ID (opsiyonel)</param>
        /// <param name="status">Onay durumu (opsiyonel)</param>
        /// <returns>Sayfalanmış yorum listesi</returns>
        public async Task<PagedList<CommentListDto>> GetCommentsAsync(int page = 1, int pageSize = 10, Guid? articleId = null, int? status = null)
        {
            try
            {
                var query = _commentRepository.Get()
                    .Where(c => !c.IsDeleted);

                // Makale filtresi
                if (articleId.HasValue)
                {
                    query = query.Where(c => c.ArticleId == articleId);
                }

                // Durum filtresi
                if (status.HasValue)
                {
                    query = query.Where(c => c.Status == status);
                }

                // CommentListDto'ya dönüştür ve sırala
                var commentQuery = query
                    .OrderByDescending(c => c.CreatedDate)
                    .Select(c => new CommentListDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        UserName = "", // UserManager ile ayrıca alınmalı
                        ArticleTitle = "", // Article repository ile ayrıca alınmalı
                        LikeCount = c.LikeCount,
                        Status = c.Status == 0 ? "pending" : c.Status == 1 ? "approved" : "rejected",
                        IpAddress = c.IpAddress,
                        UserAgent = c.UserAgent,
                        CreatedDate = c.CreatedDate,
                        IsPublish = c.IsPublish
                    });

                // PagedList kullan
                return await commentQuery.ToPagedListAsync(page - 1, pageSize, new List<Filter>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum listesi getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Makale yorumlarını getirir
        /// </summary>
        /// <param name="articleId">Makale ID</param>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <returns>Makale yorumları</returns>
        public async Task<PagedList<CommentListDto>> GetCommentsByArticleAsync(Guid articleId, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _commentRepository.Get()
                    .Where(c => !c.IsDeleted && c.ArticleId == articleId && c.Status == 1) // Onaylanmış yorumlar
                    .OrderByDescending(c => c.CreatedDate);

                // CommentListDto'ya dönüştür
                var commentQuery = query.Select(c => new CommentListDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserName = "", // UserManager ile ayrıca alınmalı
                    ArticleTitle = "", // Article repository ile ayrıca alınmalı
                    LikeCount = c.LikeCount,
                    Status = c.Status == 0 ? "pending" : c.Status == 1 ? "approved" : "rejected",
                    IpAddress = c.IpAddress,
                    UserAgent = c.UserAgent,
                    CreatedDate = c.CreatedDate,
                    IsPublish = c.IsPublish
                });

                // PagedList kullan
                return await commentQuery.ToPagedListAsync(page - 1, pageSize, new List<Filter>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale yorumları getirilirken hata oluştu. Article ID: {ArticleId}", articleId);
                throw;
            }
        }

        /// <summary>
        /// Yorum oluşturur
        /// </summary>
        /// <param name="createCommentDto">Yorum oluşturma DTO'su</param>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Oluşturulan yorum</returns>
        public async Task<CommentListDto> CreateCommentAsync(CreateCommentDto createCommentDto, Guid userId)
        {
            try
            {
                var comment = new Comment
                {
                    Content = createCommentDto.Content,
                    ArticleId = createCommentDto.ArticleId,
                    UserId = userId,
                    ParentCommentId = createCommentDto.ParentCommentId,
                    IpAddress = createCommentDto.IpAddress,
                    UserAgent = createCommentDto.UserAgent,
                    Status = 0 // Beklemede
                };

                await _commentRepository.AddAsync(comment);

                // Makale yorum sayısını artır
                var article = await _articleRepository.FindByIdAsync(createCommentDto.ArticleId);
                if (article != null)
                {
                    article.CommentCount++;
                    await _articleRepository.UpdateAsync(article);
                }

                // Tüm DB işleri bittikten sonra SaveChangesAsync çağır
                await _commentRepository.SaveChangesAsync();

                // Oluşturulan yorumu getir
                var createdComment = await _commentRepository.Get()
                    .Where(c => c.Id == comment.Id)
                    .FirstOrDefaultAsync();

                if (createdComment != null)
                {
                    var user = await _userManager.FindByIdAsync(createdComment.UserId.ToString());
                    var articleEntity = await _articleRepository.FindByIdAsync(createdComment.ArticleId);

                    return new CommentListDto
                    {
                        Id = createdComment.Id,
                        Content = createdComment.Content,
                        UserName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                        ArticleTitle = articleEntity?.Title ?? "",
                        LikeCount = createdComment.LikeCount,
                        Status = createdComment.Status == 0 ? "pending" : createdComment.Status == 1 ? "approved" : "rejected",
                        IpAddress = createdComment.IpAddress,
                        UserAgent = createdComment.UserAgent,
                        CreatedDate = createdComment.CreatedDate,
                        IsPublish = createdComment.IsPublish
                    };
                }

                throw new InvalidOperationException("Oluşturulan yorum bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum oluşturulurken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Yorum günceller
        /// </summary>
        /// <param name="updateCommentDto">Yorum güncelleme DTO'su</param>
        /// <returns>Güncellenen yorum</returns>
        public async Task<CommentListDto> UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            try
            {
                var comment = await _commentRepository.FindByIdAsync(updateCommentDto.Id);
                if (comment == null)
                    throw new InvalidOperationException("Yorum bulunamadı");

                comment.Content = updateCommentDto.Content;
                comment.Status = updateCommentDto.Status;

                await _commentRepository.UpdateAsync(comment);
                await _commentRepository.SaveChangesAsync();

                // Güncellenen yorumu getir
                var updatedComment = await _commentRepository.Get()
                    .Where(c => c.Id == comment.Id)
                    .FirstOrDefaultAsync();

                if (updatedComment != null)
                {
                    var user = await _userManager.FindByIdAsync(updatedComment.UserId.ToString());
                    var articleEntity = await _articleRepository.FindByIdAsync(updatedComment.ArticleId);

                    return new CommentListDto
                    {
                        Id = updatedComment.Id,
                        Content = updatedComment.Content,
                        UserName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                        ArticleTitle = articleEntity?.Title ?? "",
                        LikeCount = updatedComment.LikeCount,
                        Status = updatedComment.Status == 0 ? "pending" : updatedComment.Status == 1 ? "approved" : "rejected",
                        IpAddress = updatedComment.IpAddress,
                        UserAgent = updatedComment.UserAgent,
                        CreatedDate = updatedComment.CreatedDate,
                        IsPublish = updatedComment.IsPublish
                    };
                }

                throw new InvalidOperationException("Güncellenen yorum bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum güncellenirken hata oluştu. ID: {CommentId}", updateCommentDto.Id);
                throw;
            }
        }

        /// <summary>
        /// Yorum siler (soft delete)
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> DeleteCommentAsync(Guid id)
        {
            try
            {
                var comment = await _commentRepository.FindByIdAsync(id);
                if (comment == null)
                    return false;

                comment.IsDeleted = true;
                comment.ModifiedDate = DateTime.UtcNow;
                await _commentRepository.UpdateAsync(comment);

                // Makale yorum sayısını azalt
                var article = await _articleRepository.FindByIdAsync(comment.ArticleId);
                if (article != null && article.CommentCount > 0)
                {
                    article.CommentCount--;
                    await _articleRepository.UpdateAsync(article);
                }

                // Tüm DB işleri bittikten sonra SaveChangesAsync çağır
                await _commentRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum silinirken hata oluştu. ID: {CommentId}", id);
                return false;
            }
        }

        /// <summary>
        /// Yorumu geri yükler
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> RestoreCommentAsync(Guid id)
        {
            try
            {
                var comment = await _commentRepository.FindByIdAsync(id);
                if (comment == null)
                    return false;

                comment.Restore();
                await _commentRepository.UpdateAsync(comment);

                // Makale yorum sayısını artır
                var article = await _articleRepository.FindByIdAsync(comment.ArticleId);
                if (article != null)
                {
                    article.CommentCount++;
                    await _articleRepository.UpdateAsync(article);
                }

                // Tüm DB işleri bittikten sonra SaveChangesAsync çağır
                await _commentRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum geri yüklenirken hata oluştu. ID: {CommentId}", id);
                return false;
            }
        }

        /// <summary>
        /// Yorum onay durumunu günceller
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <param name="status">Onay durumu</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> UpdateCommentStatusAsync(Guid id, int status)
        {
            try
            {
                var comment = await _commentRepository.FindByIdAsync(id);
                if (comment == null)
                    return false;

                var oldStatus = comment.Status;
                comment.Status = status;

                await _commentRepository.UpdateAsync(comment);

                // Makale yorum sayısını güncelle
                var article = await _articleRepository.FindByIdAsync(comment.ArticleId);
                if (article != null)
                {
                    if (oldStatus != 1 && status == 1) // Onaylanmış
                    {
                        article.CommentCount++;
                    }
                    else if (oldStatus == 1 && status != 1) // Onayı kaldırılmış
                    {
                        article.CommentCount = Math.Max(0, article.CommentCount - 1);
                    }

                    await _articleRepository.UpdateAsync(article);
                }

                // Tüm DB işleri bittikten sonra SaveChangesAsync çağır
                await _commentRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum onay durumu güncellenirken hata oluştu. ID: {CommentId}", id);
                return false;
            }
        }

        /// <summary>
        /// Yorum beğeni sayısını artırır
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> IncrementLikeCountAsync(Guid id)
        {
            try
            {
                var comment = await _commentRepository.FindByIdAsync(id);
                if (comment == null)
                    return false;

                comment.LikeCount++;
                await _commentRepository.UpdateAsync(comment);
                await _commentRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum beğeni sayısı artırılırken hata oluştu. ID: {CommentId}", id);
                return false;
            }
        }

        /// <summary>
        /// Son yorumları getirir
        /// </summary>
        /// <param name="count">Yorum sayısı</param>
        /// <returns>Son yorumlar</returns>
        public async Task<List<CommentListDto>> GetRecentCommentsAsync(int count = 10)
        {
            try
            {
                var comments = await _commentRepository.Get()
                    .Where(c => !c.IsDeleted && c.Status == 1) // Onaylanmış yorumlar
                    .OrderByDescending(c => c.CreatedDate)
                    .Take(count)
                    .ToListAsync();

                // UserName ve ArticleTitle alanlarını doldur
                var commentDtos = new List<CommentListDto>();
                foreach (var comment in comments)
                {
                    var user = await _userManager.FindByIdAsync(comment.UserId.ToString());
                    var articleEntity = await _articleRepository.FindByIdAsync(comment.ArticleId);

                    commentDtos.Add(new CommentListDto
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        UserName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                        ArticleTitle = articleEntity?.Title ?? "",
                        LikeCount = comment.LikeCount,
                        Status = comment.Status == 0 ? "pending" : comment.Status == 1 ? "approved" : "rejected",
                        IpAddress = comment.IpAddress,
                        UserAgent = comment.UserAgent,
                        CreatedDate = comment.CreatedDate,
                        IsPublish = comment.IsPublish
                    });
                }

                return commentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Son yorumlar getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Bekleyen yorumları getirir
        /// </summary>
        /// <param name="count">Yorum sayısı</param>
        /// <returns>Bekleyen yorumlar</returns>
        public async Task<List<CommentListDto>> GetPendingCommentsAsync(int count = 10)
        {
            try
            {
                var comments = await _commentRepository.Get()
                    .Where(c => !c.IsDeleted && c.Status == 0) // Bekleyen yorumlar
                    .OrderByDescending(c => c.CreatedDate)
                    .Take(count)
                    .ToListAsync();

                // UserName ve ArticleTitle alanlarını doldur
                var commentDtos = new List<CommentListDto>();
                foreach (var comment in comments)
                {
                    var user = await _userManager.FindByIdAsync(comment.UserId.ToString());
                    var articleEntity = await _articleRepository.FindByIdAsync(comment.ArticleId);

                    commentDtos.Add(new CommentListDto
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        UserName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                        ArticleTitle = articleEntity?.Title ?? "",
                        LikeCount = comment.LikeCount,
                        Status = comment.Status == 0 ? "pending" : comment.Status == 1 ? "approved" : "rejected",
                        IpAddress = comment.IpAddress,
                        UserAgent = comment.UserAgent,
                        CreatedDate = comment.CreatedDate,
                        IsPublish = comment.IsPublish
                    });
                }

                return commentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bekleyen yorumlar getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Yorum detayını getirir
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>Yorum detayı</returns>
        public async Task<CommentListDto?> GetCommentByIdAsync(Guid id)
        {
            try
            {
                var comment = await _commentRepository.Get()
                    .Where(c => c.Id == id && !c.IsDeleted)
                    .FirstOrDefaultAsync();

                if (comment == null)
                    return null;

                var user = await _userManager.FindByIdAsync(comment.UserId.ToString());
                var article = await _articleRepository.FindByIdAsync(comment.ArticleId);

                return new CommentListDto
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    UserName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                    ArticleTitle = article?.Title ?? "",
                    LikeCount = comment.LikeCount,
                    Status = comment.Status == 0 ? "pending" : comment.Status == 1 ? "approved" : "rejected",
                    IpAddress = comment.IpAddress,
                    UserAgent = comment.UserAgent,
                    CreatedDate = comment.CreatedDate,
                    IsPublish = comment.IsPublish
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum detayı getirilirken hata oluştu. ID: {CommentId}", id);
                throw;
            }
        }
    }
}
