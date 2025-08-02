using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Newspaper.Dto.Mssql;
using Newspaper.Mssql.Services;
using Maggsoft.Core.Model.Pagination;

namespace Newspaper.Api.Controllers
{
    /// <summary>
    /// Makale işlemleri
    /// </summary>
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(
            IArticleService articleService,
            ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        /// <summary>
        /// Makaleleri listeler
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <param name="categoryId">Kategori ID</param>
        /// <param name="authorId">Yazar ID</param>
        /// <param name="isPublished">Yayınlanmış mı?</param>
        /// <returns>Makale listesi</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Result<PagedList<ArticleListDto>>>> GetArticles(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] Guid? authorId = null,
            [FromQuery] bool? isPublished = null)
        {
            try
            {
                var searchDto = new ArticleSearchDto
                {
                    SearchTerm = searchTerm,
                    CategoryId = categoryId,
                    AuthorId = authorId,
                    IsPublished = isPublished
                };

                var articles = await _articleService.GetArticlesAsync(searchDto);
                return Success(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makaleler listelenirken hata oluştu");
                return Error<PagedList<ArticleListDto>>("Makaleler listelenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Öne çıkan makaleleri getirir
        /// </summary>
        /// <param name="count">Makale sayısı</param>
        /// <returns>Öne çıkan makaleler</returns>
        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<List<ArticleListDto>>>> GetFeaturedArticles([FromQuery] int count = 5)
        {
            try
            {
                var articles = await _articleService.GetFeaturedArticlesAsync(count);
                return Success(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Öne çıkan makaleler getirilirken hata oluştu");
                return Error<List<ArticleListDto>>("Öne çıkan makaleler getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Ana sayfa makalelerini getirir
        /// </summary>
        /// <param name="count">Makale sayısı</param>
        /// <returns>Ana sayfa makaleleri</returns>
        [HttpGet("homepage")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<List<ArticleListDto>>>> GetHomePageArticles([FromQuery] int count = 10)
        {
            try
            {
                var articles = await _articleService.GetHomePageArticlesAsync(count);
                return Success(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ana sayfa makaleleri getirilirken hata oluştu");
                return Error<List<ArticleListDto>>("Ana sayfa makaleleri getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Makale detayını getirir
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>Makale detayı</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<ArticleDetailDto>>> GetArticleById(Guid id)
        {
            try
            {
                var article = await _articleService.GetArticleByIdAsync(id);
                if (article == null)
                {
                    return Error<ArticleDetailDto>("Makale bulunamadı", 404);
                }

                // Görüntülenme sayısını artır
                await _articleService.IncrementViewCountAsync(id);

                return Success(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale detayı getirilirken hata oluştu. ID: {ArticleId}", id);
                return Error<ArticleDetailDto>("Makale detayı getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Slug ile makale getirir
        /// </summary>
        /// <param name="slug">Makale slug</param>
        /// <returns>Makale detayı</returns>
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<ArticleDetailDto>>> GetArticleBySlug(string slug)
        {
            try
            {
                var article = await _articleService.GetArticleBySlugAsync(slug);
                if (article == null)
                {
                    return Error<ArticleDetailDto>("Makale bulunamadı", 404);
                }

                // Görüntülenme sayısını artır
                await _articleService.IncrementViewCountAsync(article.Id);

                return Success(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale slug ile getirilirken hata oluştu. Slug: {Slug}", slug);
                return Error<ArticleDetailDto>("Makale getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yeni makale oluşturur
        /// </summary>
        /// <param name="createArticleDto">Makale oluşturma DTO'su</param>
        /// <returns>Oluşturulan makale</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<ArticleDetailDto>>> CreateArticle([FromBody] CreateArticleDto createArticleDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Error<ArticleDetailDto>("Kullanıcı bilgisi bulunamadı", 401);
                }

                createArticleDto.AuthorId = userId.Value;
                var article = await _articleService.CreateArticleAsync(createArticleDto);
                return Success(article, "Makale başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale oluşturulurken hata oluştu");
                return Error<ArticleDetailDto>("Makale oluşturulurken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Makale günceller
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <param name="updateArticleDto">Makale güncelleme DTO'su</param>
        /// <returns>Güncellenen makale</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<ArticleDetailDto>>> UpdateArticle(Guid id, [FromBody] UpdateArticleDto updateArticleDto)
        {
            try
            {
                updateArticleDto.Id = id;
                var article = await _articleService.UpdateArticleAsync(updateArticleDto);
                return Success(article, "Makale başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale güncellenirken hata oluştu. ID: {ArticleId}", id);
                return Error<ArticleDetailDto>("Makale güncellenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Makale siler
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<object>>> DeleteArticle(Guid id)
        {
            try
            {
                var result = await _articleService.DeleteArticleAsync(id);
                if (!result)
                {
                    return Error<object>("Makale silinemedi", 400);
                }

                return Success<object>(null, "Makale başarıyla silindi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale silinirken hata oluştu. ID: {ArticleId}", id);
                return Error<object>("Makale silinirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Makaleyi geri yükler
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/restore")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<object>>> RestoreArticle(Guid id)
        {
            try
            {
                var result = await _articleService.RestoreArticleAsync(id);
                if (!result)
                {
                    return Error<object>("Makale geri yüklenemedi", 400);
                }

                return Success<object>(null, "Makale başarıyla geri yüklendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale geri yüklenirken hata oluştu. ID: {ArticleId}", id);
                return Error<object>("Makale geri yüklenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Makale beğeni sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/like")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<object>>> LikeArticle(Guid id)
        {
            try
            {
                var result = await _articleService.IncrementLikeCountAsync(id);
                if (!result)
                {
                    return Error<object>("Makale beğenilemedi", 400);
                }

                return Success<object>(null, "Makale beğenildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale beğenilirken hata oluştu. ID: {ArticleId}", id);
                return Error<object>("Makale beğenilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Makale paylaşım sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/share")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<object>>> ShareArticle(Guid id)
        {
            try
            {
                var result = await _articleService.IncrementShareCountAsync(id);
                if (!result)
                {
                    return Error<object>("Makale paylaşılamadı", 400);
                }

                return Success<object>(null, "Makale paylaşıldı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale paylaşılırken hata oluştu. ID: {ArticleId}", id);
                return Error<object>("Makale paylaşılırken hata oluştu", 500);
            }
        }
    }
} 