using Microsoft.Extensions.Logging;
using Newspaper.Data.Mssql;
using Newspaper.Dto.Mssql;
using Maggsoft.Mssql.Services;
using Maggsoft.Mssql.Repository;
using Microsoft.EntityFrameworkCore;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.Model;
using Maggsoft.Core.Extensions;
using Maggsoft.Core.IoC;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Kategori servis implementasyonu
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IMssqlRepository<Category> _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IMssqlRepository<Category> categoryRepository,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        /// <summary>
        /// Kategori listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <param name="parentId">Üst kategori ID</param>
        /// <returns>Sayfalanmış kategori listesi</returns>
        public async Task<PagedList<CategoryListDto>> GetCategoriesAsync(int page = 1, int pageSize = 10, string? searchTerm = null, Guid? parentId = null)
        {
            try
            {
                var query = _categoryRepository.Get()
                .Where(c => !c.IsDeleted);

                // Arama filtresi
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(c => c.Name.Contains(searchTerm) || c.Description!.Contains(searchTerm));
                }

                // Üst kategori filtresi
                if (parentId.HasValue)
                {
                    query = query.Where(c => c.ParentCategoryId == parentId);
                }

                // CategoryListDto'ya dönüştür ve sırala
                var categoryQuery = query
                    .OrderBy(c => c.Order)
                    .ThenBy(c => c.Name)
                    .Select(c => new CategoryListDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Icon = c.Icon,
                        Color = c.Color,
                        CreatedDate = c.CreatedDate,
                        IsPublish = c.IsPublish,
                        ArticleCount = c.Articles.Count(a => !a.IsDeleted)
                    });

                // PagedList kullan
                return await categoryQuery.ToPagedListAsync(page - 1, pageSize, new List<Filter>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori listesi getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Tüm kategorileri getirir (hiyerarşik)
        /// </summary>
        /// <returns>Kategori listesi</returns>
        public async Task<List<CategoryListDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.Get()
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.Order)
                    .ThenBy(c => c.Name)
                    .Select(c => new CategoryListDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Icon = c.Icon,
                        Color = c.Color,
                        CreatedDate = c.CreatedDate,
                        IsPublish = c.IsPublish,
                        ArticleCount = c.Articles.Count(a => !a.IsDeleted)
                    })
                    .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm kategoriler getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Kategori detayını getirir
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>Kategori detayı</returns>
        public async Task<CategoryDetailDto?> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var category = await _categoryRepository.Get()
                    .Where(c => c.Id == id && !c.IsDeleted)
                    .Select(c => new CategoryDetailDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Slug = c.Slug,
                        Icon = c.Icon,
                        Color = c.Color,
                        ParentCategoryId = c.ParentCategoryId,
                        Order = c.Order,
                        ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                        SubCategoryCount = c.SubCategories.Count(sc => !sc.IsDeleted),
                        ArticleCount = c.Articles.Count(a => !a.IsDeleted),
                        CreatedDate = c.CreatedDate,
                        ModifiedDate = c.ModifiedDate,
                        IsPublish = c.IsPublish
                    })
                    .FirstOrDefaultAsync();

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori detayı getirilirken hata oluştu. ID: {CategoryId}", id);
                throw;
            }
        }

        /// <summary>
        /// Kategori oluşturur
        /// </summary>
        /// <param name="createCategoryDto">Kategori oluşturma DTO'su</param>
        /// <returns>Oluşturulan kategori</returns>
        public async Task<CategoryDetailDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            try
            {
                var category = new Category
                {
                    Name = createCategoryDto.Name,
                    Description = createCategoryDto.Description,
                    Slug = createCategoryDto.Slug,
                    Icon = createCategoryDto.Icon,
                    Color = createCategoryDto.Color,
                    ParentCategoryId = createCategoryDto.ParentCategoryId,
                    Order = createCategoryDto.Order
                };

                await _categoryRepository.AddAsync(category);

                return await GetCategoryByIdAsync(category.Id) ?? throw new InvalidOperationException("Oluşturulan kategori bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Kategori günceller
        /// </summary>
        /// <param name="updateCategoryDto">Kategori güncelleme DTO'su</param>
        /// <returns>Güncellenen kategori</returns>
        public async Task<CategoryDetailDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(updateCategoryDto.Id);
                if (category == null)
                    throw new InvalidOperationException("Kategori bulunamadı");

                category.Name = updateCategoryDto.Name;
                category.Description = updateCategoryDto.Description;
                category.Slug = updateCategoryDto.Slug;
                category.Icon = updateCategoryDto.Icon;
                category.Color = updateCategoryDto.Color;
                category.ParentCategoryId = updateCategoryDto.ParentCategoryId;
                category.Order = updateCategoryDto.Order;

                await _categoryRepository.UpdateAsync(category);

                return await GetCategoryByIdAsync(category.Id) ?? throw new InvalidOperationException("Güncellenen kategori bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori güncellenirken hata oluştu. ID: {CategoryId}", updateCategoryDto.Id);
                throw;
            }
        }

        /// <summary>
        /// Kategori siler (soft delete)
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category == null)
                    return false;

                category.IsDeleted = true;
                category.ModifiedDate = DateTime.UtcNow;
                await _categoryRepository.UpdateAsync(category);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori silinirken hata oluştu. ID: {CategoryId}", id);
                return false;
            }
        }

        /// <summary>
        /// Kategoriyi geri yükler
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> RestoreCategoryAsync(Guid id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category == null)
                    return false;

                category.Restore();
                await _categoryRepository.UpdateAsync(category);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori geri yüklenirken hata oluştu. ID: {CategoryId}", id);
                return false;
            }
        }

        /// <summary>
        /// Alt kategorileri getirir
        /// </summary>
        /// <param name="parentId">Üst kategori ID</param>
        /// <returns>Alt kategori listesi</returns>
        public async Task<List<CategoryListDto>> GetSubCategoriesAsync(Guid parentId)
        {
            try
            {
                var subCategories = await _categoryRepository.Get()
                    .Where(c => c.ParentCategoryId == parentId && !c.IsDeleted)
                    .OrderBy(c => c.Order)
                    .ThenBy(c => c.Name)
                    .Select(c => new CategoryListDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Icon = c.Icon,
                        Color = c.Color,
                        CreatedDate = c.CreatedDate,
                        IsPublish = c.IsPublish,
                        ArticleCount = c.Articles.Count(a => !a.IsDeleted)
                    })
                    .ToListAsync();

                return subCategories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Alt kategoriler getirilirken hata oluştu. Parent ID: {ParentId}", parentId);
                throw;
            }
        }
    }
} 