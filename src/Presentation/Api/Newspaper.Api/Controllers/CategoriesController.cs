using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Newspaper.Dto.Mssql;
using Newspaper.Mssql.Services;
using Maggsoft.Core.Model.Pagination;

namespace Newspaper.Api.Controllers
{
    /// <summary>
    /// Kategori işlemleri
    /// </summary>
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Kategorileri listeler
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <param name="parentId">Üst kategori ID</param>
        /// <returns>Kategori listesi</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Result<PagedList<CategoryListDto>>>> GetCategories(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] Guid? parentId = null)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync(page, pageSize, searchTerm, parentId);
                return Success(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriler listelenirken hata oluştu");
                return Error<PagedList<CategoryListDto>>("Kategoriler listelenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Tüm kategorileri listeler
        /// </summary>
        /// <returns>Kategori listesi</returns>
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<List<CategoryListDto>>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Success(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tüm kategoriler listelenirken hata oluştu");
                return Error<List<CategoryListDto>>("Tüm kategoriler listelenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Kategori detayını getirir
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>Kategori detayı</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<CategoryDetailDto>>> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return Error<CategoryDetailDto>("Kategori bulunamadı", 404);
                }

                return Success(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori detayı getirilirken hata oluştu. ID: {CategoryId}", id);
                return Error<CategoryDetailDto>("Kategori detayı getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Alt kategorileri getirir
        /// </summary>
        /// <param name="parentId">Üst kategori ID</param>
        /// <returns>Alt kategori listesi</returns>
        [HttpGet("subcategories/{parentId}")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<List<CategoryListDto>>>> GetSubCategories(Guid parentId)
        {
            try
            {
                var subCategories = await _categoryService.GetSubCategoriesAsync(parentId);
                return Success(subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Alt kategoriler getirilirken hata oluştu. Parent ID: {ParentId}", parentId);
                return Error<List<CategoryListDto>>("Alt kategoriler getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Yeni kategori oluşturur
        /// </summary>
        /// <param name="createCategoryDto">Kategori oluşturma DTO'su</param>
        /// <returns>Oluşturulan kategori</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<CategoryDetailDto>>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
                return Success(category, "Kategori başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken hata oluştu");
                return Error<CategoryDetailDto>("Kategori oluşturulurken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Kategori günceller
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <param name="updateCategoryDto">Kategori güncelleme DTO'su</param>
        /// <returns>Güncellenen kategori</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<Result<CategoryDetailDto>>> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                updateCategoryDto.Id = id;
                var category = await _categoryService.UpdateCategoryAsync(updateCategoryDto);
                return Success(category, "Kategori başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori güncellenirken hata oluştu. ID: {CategoryId}", id);
                return Error<CategoryDetailDto>("Kategori güncellenirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Kategori siler
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result<object>>> DeleteCategory(Guid id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                {
                    return Error<object>("Kategori silinemedi", 400);
                }

                return Success<object>(null, "Kategori başarıyla silindi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori silinirken hata oluştu. ID: {CategoryId}", id);
                return Error<object>("Kategori silinirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Kategoriyi geri yükler
        /// </summary>
        /// <param name="id">Kategori ID</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("{id}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Result<object>>> RestoreCategory(Guid id)
        {
            try
            {
                var result = await _categoryService.RestoreCategoryAsync(id);
                if (!result)
                {
                    return Error<object>("Kategori geri yüklenemedi", 400);
                }

                return Success<object>(null, "Kategori başarıyla geri yüklendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori geri yüklenirken hata oluştu. ID: {CategoryId}", id);
                return Error<object>("Kategori geri yüklenirken hata oluştu", 500);
            }
        }
    }
}
