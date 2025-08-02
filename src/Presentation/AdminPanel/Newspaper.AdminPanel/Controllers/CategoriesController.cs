using Maggsoft.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Models;
using Newspaper.Dto.Mssql;
using Maggsoft.Framework.HttpClientApi;

namespace Newspaper.AdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class CategoriesController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;

        public CategoriesController(IMaggsoftHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Kategori Yönetimi";

            try
            {
                var response = await _httpClient.GetAsync<Result<List<CategoryDto>>>("api/category");
                if (response.IsSuccess)
                {
                    return View(response.Data);
                }

                TempData["Error"] = "Kategoriler yüklenirken hata oluştu.";
                return View(new List<CategoryDto>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kategoriler yüklenirken hata oluştu: " + ex.Message;
                return View(new List<CategoryDto>());
            }
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Kategori Oluştur";
            return View(new CreateCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createCategoryDto = new CreateCategoryDto
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Slug = model.Slug
                    };

                    var response = await _httpClient.PostAsync<Result<CategoryDto>>("api/category", createCategoryDto);
                    if (response.IsSuccess)
                    {
                        TempData["Success"] = "Kategori başarıyla oluşturuldu.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = response.Message ?? "Kategori oluşturulurken hata oluştu.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Kategori oluşturulurken hata oluştu: " + ex.Message;
                }
            }

            ViewData["Title"] = "Yeni Kategori Oluştur";
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["Title"] = "Kategori Düzenle";

            try
            {
                var response = await _httpClient.GetAsync<Result<CategoryDto>>($"api/category/{id}");
                if (response.IsSuccess)
                {
                    var model = new EditCategoryViewModel
                    {
                        Id = response.Data.Id,
                        Name = response.Data.Name,
                        Description = response.Data.Description,
                        Slug = response.Data.Slug,
                        IsActive = response.Data.IsActive
                    };
                    return View(model);
                }

                TempData["Error"] = "Kategori bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kategori yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateCategoryDto = new UpdateCategoryDto
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Slug = model.Slug,
                        IsActive = model.IsActive
                    };

                    var response = await _httpClient.PutAsync<Result<CategoryDto>>($"api/category/{model.Id}", updateCategoryDto);
                    if (response.IsSuccess)
                    {
                        TempData["Success"] = "Kategori başarıyla güncellendi.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = response.Message ?? "Kategori güncellenirken hata oluştu.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Kategori güncellenirken hata oluştu: " + ex.Message;
                }
            }

            ViewData["Title"] = "Kategori Düzenle";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync<Result<object>>($"api/category/{id}");
                if (response.IsSuccess)
                {
                    TempData["Success"] = "Kategori başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = response.Message ?? "Kategori silinirken hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kategori silinirken hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

