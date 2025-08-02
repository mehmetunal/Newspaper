using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data.Mssql;
using Newspaper.Dto.Mssql;
using Maggsoft.Mssql.Repository;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.Model;
using Maggsoft.Core.Extensions;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Makale servis implementasyonu
    /// </summary>
    public class ArticleService : IArticleService
    {
        private readonly IMssqlRepository<Article> _articleRepository;
        private readonly IMssqlRepository<ArticleTag> _articleTagRepository;
        private readonly IMssqlRepository<Tag> _tagRepository;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(
            IMssqlRepository<Article> articleRepository,
            IMssqlRepository<ArticleTag> articleTagRepository,
            IMssqlRepository<Tag> tagRepository,
            ILogger<ArticleService> logger)
        {
            _articleRepository = articleRepository;
            _articleTagRepository = articleTagRepository;
            _tagRepository = tagRepository;
            _logger = logger;
        }

        /// <summary>
        /// Makale listesini getirir
        /// </summary>
        /// <param name="searchDto">Arama parametreleri</param>
        /// <returns>Sayfalanmış makale listesi</returns>
        public async Task<PagedList<ArticleListDto>> GetArticlesAsync(ArticleSearchDto searchDto)
        {
            try
            {
                var query = _articleRepository.Get()
                    .Where(a => !a.IsDeleted);

                // Arama filtresi
                if (!string.IsNullOrEmpty(searchDto.SearchTerm))
                {
                    query = query.Where(a => 
                        a.Title.Contains(searchDto.SearchTerm) || 
                        a.Summary!.Contains(searchDto.SearchTerm) ||
                        a.Content.Contains(searchDto.SearchTerm));
                }

                // Kategori filtresi
                if (searchDto.CategoryId.HasValue)
                {
                    query = query.Where(a => a.CategoryId == searchDto.CategoryId);
                }

                // Yazar filtresi
                if (searchDto.AuthorId.HasValue)
                {
                    query = query.Where(a => a.AuthorId == searchDto.AuthorId);
                }

                // Yayın durumu filtresi
                if (searchDto.Status.HasValue)
                {
                    query = query.Where(a => a.Status == searchDto.Status);
                }

                // Öne çıkan makale filtresi
                if (searchDto.IsFeatured.HasValue)
                {
                    query = query.Where(a => a.IsFeatured == searchDto.IsFeatured);
                }

                // Tarih filtresi
                if (searchDto.StartDate.HasValue)
                {
                    query = query.Where(a => a.CreatedDate >= searchDto.StartDate);
                }

                if (searchDto.EndDate.HasValue)
                {
                    query = query.Where(a => a.CreatedDate <= searchDto.EndDate);
                }

                // Sıralama
                query = searchDto.SortBy?.ToLower() switch
                {
                    "title" => searchDto.SortDirection == "asc" ? query.OrderBy(a => a.Title) : query.OrderByDescending(a => a.Title),
                    "createddate" => searchDto.SortDirection == "asc" ? query.OrderBy(a => a.CreatedDate) : query.OrderByDescending(a => a.CreatedDate),
                    "publishedat" => searchDto.SortDirection == "asc" ? query.OrderBy(a => a.PublishedAt) : query.OrderByDescending(a => a.PublishedAt),
                    "viewcount" => searchDto.SortDirection == "asc" ? query.OrderBy(a => a.ViewCount) : query.OrderByDescending(a => a.ViewCount),
                    "likecount" => searchDto.SortDirection == "asc" ? query.OrderBy(a => a.LikeCount) : query.OrderByDescending(a => a.LikeCount),
                    _ => query.OrderByDescending(a => a.CreatedDate)
                };

                // ArticleListDto'ya dönüştür
                var articleQuery = query.Select(a => new ArticleListDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Summary = a.Summary,
                    Slug = a.Slug,
                    CoverImageUrl = a.CoverImageUrl,
                    AuthorName = a.Author.FullName,
                    CategoryName = a.Category.Name,
                    PublishedAt = a.PublishedAt,
                    ViewCount = a.ViewCount,
                    LikeCount = a.LikeCount,
                    CommentCount = a.CommentCount,
                    IsFeatured = a.IsFeatured,
                    ReadingTime = a.ReadingTime,
                    CreatedDate = a.CreatedDate,
                    IsPublish = a.IsPublish
                });

                // PagedList kullan
                return await articleQuery.ToPagedListAsync(searchDto.Page - 1, searchDto.PageSize, new List<Filter>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale listesi getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Makale detayını getirir
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>Makale detayı</returns>
        public async Task<ArticleDetailDto?> GetArticleByIdAsync(Guid id)
        {
            try
            {
                var article = await _articleRepository.Get()
                    .Where(a => a.Id == id && !a.IsDeleted)
                    .FirstOrDefaultAsync();

                if (article == null)
                    return null;

                return new ArticleDetailDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    Summary = article.Summary,
                    Slug = article.Slug,
                    CoverImageUrl = article.CoverImageUrl,
                    MetaKeywords = article.MetaKeywords,
                    MetaDescription = article.MetaDescription,
                    AuthorId = article.AuthorId,
                    AuthorName = article.Author.FullName,
                    CategoryId = article.CategoryId,
                    CategoryName = article.Category.Name,
                    PublishedAt = article.PublishedAt,
                    ViewCount = article.ViewCount,
                    LikeCount = article.LikeCount,
                    CommentCount = article.CommentCount,
                    ShareCount = article.ShareCount,
                    IsFeatured = article.IsFeatured,
                    IsOnHomePage = article.IsOnHomePage,
                    Status = article.Status,
                    ReadingTime = article.ReadingTime,
                    Tags = article.ArticleTags.Select(at => at.Tag.Name).ToList(),
                    CreatedDate = article.CreatedDate,
                    ModifiedDate = article.ModifiedDate,
                    IsPublish = article.IsPublish
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale detayı getirilirken hata oluştu. ID: {ArticleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Makale detayını slug ile getirir
        /// </summary>
        /// <param name="slug">Makale slug</param>
        /// <returns>Makale detayı</returns>
        public async Task<ArticleDetailDto?> GetArticleBySlugAsync(string slug)
        {
            try
            {
                var article = await _articleRepository.Get()
                    .Where(a => a.Slug == slug && !a.IsDeleted && a.IsPublish)
                    .Include(a => a.Author)
                    .Include(a => a.Category)
                    .Include(a => a.ArticleTags)
                    .ThenInclude(at => at.Tag)
                    .FirstOrDefaultAsync();

                if (article == null)
                    return null;

                return new ArticleDetailDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    Summary = article.Summary,
                    Slug = article.Slug,
                    CoverImageUrl = article.CoverImageUrl,
                    MetaKeywords = article.MetaKeywords,
                    MetaDescription = article.MetaDescription,
                    AuthorId = article.AuthorId,
                    AuthorName = article.Author.FullName,
                    CategoryId = article.CategoryId,
                    CategoryName = article.Category.Name,
                    PublishedAt = article.PublishedAt,
                    ViewCount = article.ViewCount,
                    LikeCount = article.LikeCount,
                    CommentCount = article.CommentCount,
                    ShareCount = article.ShareCount,
                    IsFeatured = article.IsFeatured,
                    IsOnHomePage = article.IsOnHomePage,
                    Status = article.Status,
                    ReadingTime = article.ReadingTime,
                    Tags = article.ArticleTags.Select(at => at.Tag.Name).ToList(),
                    CreatedDate = article.CreatedDate,
                    ModifiedDate = article.ModifiedDate,
                    IsPublish = article.IsPublish
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale detayı slug ile getirilirken hata oluştu. Slug: {Slug}", slug);
                throw;
            }
        }

        /// <summary>
        /// Makale oluşturur
        /// </summary>
        /// <param name="createArticleDto">Makale oluşturma DTO'su</param>
        /// <returns>Oluşturulan makale</returns>
        public async Task<ArticleDetailDto> CreateArticleAsync(CreateArticleDto createArticleDto)
        {
            try
            {
                var article = new Article
                {
                    Title = createArticleDto.Title,
                    Content = createArticleDto.Content,
                    Summary = createArticleDto.Summary,
                    Slug = createArticleDto.Slug,
                    CoverImageUrl = createArticleDto.CoverImageUrl,
                    MetaKeywords = createArticleDto.MetaKeywords,
                    MetaDescription = createArticleDto.MetaDescription,
                    CategoryId = createArticleDto.CategoryId,
                    PublishedAt = createArticleDto.PublishedAt,
                    IsFeatured = createArticleDto.IsFeatured,
                    IsOnHomePage = createArticleDto.IsOnHomePage,
                    Status = createArticleDto.Status,
                    ReadingTime = createArticleDto.ReadingTime
                };

                await _articleRepository.AddAsync(article);

                // Etiketleri ekle
                if (createArticleDto.TagIds.Any())
                {
                    var articleTags = createArticleDto.TagIds.Select(tagId => new ArticleTag
                    {
                        ArticleId = article.Id,
                        TagId = tagId
                    }).ToList();

                    await _articleTagRepository.AddRangeAsync(articleTags);
                }

                // Tüm DB işleri bittikten sonra SaveChangesAsync çağır
                await _articleRepository.SaveChangesAsync();

                return await GetArticleByIdAsync(article.Id) ?? throw new InvalidOperationException("Oluşturulan makale bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale oluşturulurken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Makale günceller
        /// </summary>
        /// <param name="updateArticleDto">Makale güncelleme DTO'su</param>
        /// <returns>Güncellenen makale</returns>
        public async Task<ArticleDetailDto> UpdateArticleAsync(UpdateArticleDto updateArticleDto)
        {
            try
            {
                var article = await _articleRepository.FindByIdAsync(updateArticleDto.Id);
                if (article == null)
                    throw new InvalidOperationException("Makale bulunamadı");

                article.Title = updateArticleDto.Title;
                article.Content = updateArticleDto.Content;
                article.Summary = updateArticleDto.Summary;
                article.Slug = updateArticleDto.Slug;
                article.CoverImageUrl = updateArticleDto.CoverImageUrl;
                article.MetaKeywords = updateArticleDto.MetaKeywords;
                article.MetaDescription = updateArticleDto.MetaDescription;
                article.CategoryId = updateArticleDto.CategoryId;
                article.PublishedAt = updateArticleDto.PublishedAt;
                article.IsFeatured = updateArticleDto.IsFeatured;
                article.IsOnHomePage = updateArticleDto.IsOnHomePage;
                article.Status = updateArticleDto.Status;
                article.ReadingTime = updateArticleDto.ReadingTime;

                await _articleRepository.UpdateAsync(article);

                // Mevcut etiketleri sil
                var existingTags = await _articleTagRepository.Get()
                    .Where(at => at.ArticleId == article.Id)
                    .ToListAsync();

                await _articleTagRepository.DeleteAsync(existingTags);

                // Yeni etiketleri ekle
                if (updateArticleDto.TagIds.Any())
                {
                    var articleTags = updateArticleDto.TagIds.Select(tagId => new ArticleTag
                    {
                        ArticleId = article.Id,
                        TagId = tagId
                    }).ToList();

                    await _articleTagRepository.AddRangeAsync(articleTags);
                }

                // Tüm DB işleri bittikten sonra SaveChangesAsync çağır
                await _articleRepository.SaveChangesAsync();

                return await GetArticleByIdAsync(article.Id) ?? throw new InvalidOperationException("Güncellenen makale bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale güncellenirken hata oluştu. ID: {ArticleId}", updateArticleDto.Id);
                throw;
            }
        }

        /// <summary>
        /// Makale siler (soft delete)
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> DeleteArticleAsync(Guid id)
        {
            try
            {
                var article = await _articleRepository.FindByIdAsync(id);
                if (article == null)
                    return false;

                article.IsDeleted = true;
                article.ModifiedDate = DateTime.UtcNow;
                await _articleRepository.UpdateAsync(article);
                await _articleRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale silinirken hata oluştu. ID: {ArticleId}", id);
                return false;
            }
        }

        /// <summary>
        /// Makaleyi geri yükler
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> RestoreArticleAsync(Guid id)
        {
            try
            {
                var article = await _articleRepository.FindByIdAsync(id);
                if (article == null)
                    return false;

                article.Restore();
                await _articleRepository.UpdateAsync(article);
                await _articleRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale geri yüklenirken hata oluştu. ID: {ArticleId}", id);
                return false;
            }
        }

        /// <summary>
        /// Öne çıkan makaleleri getirir
        /// </summary>
        /// <param name="count">Makale sayısı</param>
        /// <returns>Öne çıkan makaleler</returns>
        public async Task<List<ArticleListDto>> GetFeaturedArticlesAsync(int count = 5)
        {
            try
            {
                var articles = await _articleRepository.Get()
                    .Where(a => !a.IsDeleted && a.IsPublish && a.IsFeatured)
                    .Include(a => a.Author)
                    .Include(a => a.Category)
                    .OrderByDescending(a => a.PublishedAt ?? a.CreatedDate)
                    .Take(count)
                    .Select(a => new ArticleListDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Summary = a.Summary,
                        Slug = a.Slug,
                        CoverImageUrl = a.CoverImageUrl,
                        AuthorName = a.Author.FullName,
                        CategoryName = a.Category.Name,
                        PublishedAt = a.PublishedAt,
                        ViewCount = a.ViewCount,
                        LikeCount = a.LikeCount,
                        CommentCount = a.CommentCount,
                        IsFeatured = a.IsFeatured,
                        ReadingTime = a.ReadingTime,
                        CreatedDate = a.CreatedDate,
                        IsPublish = a.IsPublish
                    })
                    .ToListAsync();

                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Öne çıkan makaleler getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Ana sayfa makalelerini getirir
        /// </summary>
        /// <param name="count">Makale sayısı</param>
        /// <returns>Ana sayfa makaleleri</returns>
        public async Task<List<ArticleListDto>> GetHomePageArticlesAsync(int count = 10)
        {
            try
            {
                var articles = await _articleRepository.Get()
                    .Where(a => !a.IsDeleted && a.IsPublish && a.IsOnHomePage)
                    .Include(a => a.Author)
                    .Include(a => a.Category)
                    .OrderByDescending(a => a.PublishedAt ?? a.CreatedDate)
                    .Take(count)
                    .Select(a => new ArticleListDto
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Summary = a.Summary,
                        Slug = a.Slug,
                        CoverImageUrl = a.CoverImageUrl,
                        AuthorName = a.Author.FullName,
                        CategoryName = a.Category.Name,
                        PublishedAt = a.PublishedAt,
                        ViewCount = a.ViewCount,
                        LikeCount = a.LikeCount,
                        CommentCount = a.CommentCount,
                        IsFeatured = a.IsFeatured,
                        ReadingTime = a.ReadingTime,
                        CreatedDate = a.CreatedDate,
                        IsPublish = a.IsPublish
                    })
                    .ToListAsync();

                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ana sayfa makaleleri getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Kategori makalelerini getirir
        /// </summary>
        /// <param name="categoryId">Kategori ID</param>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <returns>Kategori makaleleri</returns>
        public async Task<PagedList<ArticleListDto>> GetArticlesByCategoryAsync(Guid categoryId, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _articleRepository.Get()
                    .Where(a => !a.IsDeleted && a.IsPublish && a.CategoryId == categoryId)
                    .Include(a => a.Author)
                    .Include(a => a.Category)
                    .OrderByDescending(a => a.PublishedAt ?? a.CreatedDate);

                // ArticleListDto'ya dönüştür
                var articleQuery = query.Select(a => new ArticleListDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Summary = a.Summary,
                    Slug = a.Slug,
                    CoverImageUrl = a.CoverImageUrl,
                    AuthorName = a.Author.FullName,
                    CategoryName = a.Category.Name,
                    PublishedAt = a.PublishedAt,
                    ViewCount = a.ViewCount,
                    LikeCount = a.LikeCount,
                    CommentCount = a.CommentCount,
                    IsFeatured = a.IsFeatured,
                    ReadingTime = a.ReadingTime,
                    CreatedDate = a.CreatedDate,
                    IsPublish = a.IsPublish
                });

                // PagedList kullan
                return await articleQuery.ToPagedListAsync(page - 1, pageSize, new List<Filter>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori makaleleri getirilirken hata oluştu. Category ID: {CategoryId}", categoryId);
                throw;
            }
        }

        /// <summary>
        /// Makale görüntülenme sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> IncrementViewCountAsync(Guid id)
        {
            try
            {
                var article = await _articleRepository.FindByIdAsync(id);
                if (article == null)
                    return false;

                article.ViewCount++;
                await _articleRepository.UpdateAsync(article);
                await _articleRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale görüntülenme sayısı artırılırken hata oluştu. ID: {ArticleId}", id);
                return false;
            }
        }

        /// <summary>
        /// Makale beğeni sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> IncrementLikeCountAsync(Guid id)
        {
            try
            {
                var article = await _articleRepository.FindByIdAsync(id);
                if (article == null)
                    return false;

                article.LikeCount++;
                await _articleRepository.UpdateAsync(article);
                await _articleRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale beğeni sayısı artırılırken hata oluştu. ID: {ArticleId}", id);
                return false;
            }
        }

        /// <summary>
        /// Makale paylaşım sayısını artırır
        /// </summary>
        /// <param name="id">Makale ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> IncrementShareCountAsync(Guid id)
        {
            try
            {
                var article = await _articleRepository.FindByIdAsync(id);
                if (article == null)
                    return false;

                article.ShareCount++;
                await _articleRepository.UpdateAsync(article);
                await _articleRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale paylaşım sayısı artırılırken hata oluştu. ID: {ArticleId}", id);
                return false;
            }
        }
    }
} 