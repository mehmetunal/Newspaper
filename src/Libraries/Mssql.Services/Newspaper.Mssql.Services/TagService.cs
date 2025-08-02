using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data.Mssql;
using Newspaper.Dto.Mssql;
using Maggsoft.Mssql.Services;
using Maggsoft.Mssql.Repository;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.Model;
using Maggsoft.Core.Extensions;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Etiket servis implementasyonu
    /// </summary>
    public class TagService : ITagService
    {
        private readonly IMssqlRepository<Tag> _tagRepository;
        private readonly ILogger<TagService> _logger;

        public TagService(
            IMssqlRepository<Tag> tagRepository,
            ILogger<TagService> logger)
        {
            _tagRepository = tagRepository;
            _logger = logger;
        }

        /// <summary>
        /// Etiket listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sayfalanmış etiket listesi</returns>
        public async Task<PagedList<TagListDto>> GetTagsAsync(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            try
            {
                var query = _tagRepository.Get().Where(t => !t.IsDeleted);

                // Arama filtresi
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(t => t.Name.Contains(searchTerm) || t.Description!.Contains(searchTerm));
                }

                // TagListDto'ya dönüştür ve sırala
                var tagQuery = query
                    .OrderByDescending(t => t.UsageCount)
                    .ThenBy(t => t.Name)
                    .Select(t => new TagListDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Slug = t.Slug,
                        UsageCount = t.UsageCount,
                        CreatedDate = t.CreatedDate,
                        IsPublish = t.IsPublish
                    });

                // PagedList kullan
                return await tagQuery.ToPagedListAsync(page - 1, pageSize, new List<Filter>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket listesi getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Tüm etiketleri getirir
        /// </summary>
        /// <returns>Etiket listesi</returns>
        public async Task<List<TagListDto>> GetAllTagsAsync()
        {
            try
            {
                var tags = await _tagRepository.Get()
                    .Where(t => !t.IsDeleted)
                    .OrderByDescending(t => t.UsageCount)
                    .ThenBy(t => t.Name)
                    .Select(t => new TagListDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Slug = t.Slug,
                        UsageCount = t.UsageCount,
                        CreatedDate = t.CreatedDate,
                        IsPublish = t.IsPublish
                    })
                    .ToListAsync();

                return tags;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm etiketler getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Etiket detayını getirir
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>Etiket detayı</returns>
        public async Task<TagDetailDto?> GetTagByIdAsync(Guid id)
        {
            try
            {
                var tag = await _tagRepository.Get()
                    .Where(t => t.Id == id && !t.IsDeleted)
                    .Select(t => new TagDetailDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Slug = t.Slug,
                        Description = t.Description,
                        UsageCount = t.UsageCount,
                        ArticleCount = t.ArticleTags.Count(at => !at.IsDeleted),
                        CreatedDate = t.CreatedDate,
                        ModifiedDate = t.ModifiedDate,
                        IsPublish = t.IsPublish
                    })
                    .FirstOrDefaultAsync();

                return tag;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket detayı getirilirken hata oluştu. ID: {TagId}", id);
                throw;
            }
        }

        /// <summary>
        /// Etiket oluşturur
        /// </summary>
        /// <param name="createTagDto">Etiket oluşturma DTO'su</param>
        /// <returns>Oluşturulan etiket</returns>
        public async Task<TagDetailDto> CreateTagAsync(CreateTagDto createTagDto)
        {
            try
            {
                var tag = new Tag
                {
                    Name = createTagDto.Name,
                    Slug = createTagDto.Slug,
                    Description = createTagDto.Description
                };

                await _tagRepository.AddAsync(tag);
                await _tagRepository.SaveChangesAsync();

                return await GetTagByIdAsync(tag.Id) ?? throw new InvalidOperationException("Oluşturulan etiket bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket oluşturulurken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Etiket günceller
        /// </summary>
        /// <param name="updateTagDto">Etiket güncelleme DTO'su</param>
        /// <returns>Güncellenen etiket</returns>
        public async Task<TagDetailDto> UpdateTagAsync(UpdateTagDto updateTagDto)
        {
            try
            {
                var tag = await _tagRepository.FindByIdAsync(updateTagDto.Id);
                if (tag == null)
                    throw new InvalidOperationException("Etiket bulunamadı");

                tag.Name = updateTagDto.Name;
                tag.Slug = updateTagDto.Slug;
                tag.Description = updateTagDto.Description;

                await _tagRepository.UpdateAsync(tag);
                await _tagRepository.SaveChangesAsync();

                return await GetTagByIdAsync(tag.Id) ?? throw new InvalidOperationException("Güncellenen etiket bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket güncellenirken hata oluştu. ID: {TagId}", updateTagDto.Id);
                throw;
            }
        }

        /// <summary>
        /// Etiket siler (soft delete)
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> DeleteTagAsync(Guid id)
        {
            try
            {
                var tag = await _tagRepository.FindByIdAsync(id);
                if (tag == null)
                    return false;

                tag.IsDeleted = true;
                tag.ModifiedDate = DateTime.UtcNow;
                await _tagRepository.UpdateAsync(tag);
                await _tagRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket silinirken hata oluştu. ID: {TagId}", id);
                return false;
            }
        }

        /// <summary>
        /// Etiketi geri yükler
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> RestoreTagAsync(Guid id)
        {
            try
            {
                var tag = await _tagRepository.FindByIdAsync(id);
                if (tag == null)
                    return false;

                tag.Restore();
                await _tagRepository.UpdateAsync(tag);
                await _tagRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket geri yüklenirken hata oluştu. ID: {TagId}", id);
                return false;
            }
        }

        /// <summary>
        /// Popüler etiketleri getirir
        /// </summary>
        /// <param name="count">Etiket sayısı</param>
        /// <returns>Popüler etiketler</returns>
        public async Task<List<TagListDto>> GetPopularTagsAsync(int count = 10)
        {
            try
            {
                var tags = await _tagRepository.Get()
                    .Where(t => !t.IsDeleted && t.IsPublish)
                    .OrderByDescending(t => t.UsageCount)
                    .Take(count)
                    .Select(t => new TagListDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Slug = t.Slug,
                        UsageCount = t.UsageCount,
                        CreatedDate = t.CreatedDate,
                        IsPublish = t.IsPublish
                    })
                    .ToListAsync();

                return tags;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Popüler etiketler getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Etiket kullanım sayısını artırır
        /// </summary>
        /// <param name="id">Etiket ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> IncrementUsageCountAsync(Guid id)
        {
            try
            {
                var tag = await _tagRepository.FindByIdAsync(id);
                if (tag == null)
                    return false;

                tag.UsageCount++;
                await _tagRepository.UpdateAsync(tag);
                await _tagRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket kullanım sayısı artırılırken hata oluştu. ID: {TagId}", id);
                return false;
            }
        }
    }
} 