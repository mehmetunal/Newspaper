using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Models;
using Newspaper.Dto.Mssql;
using Maggsoft.Framework.HttpClientApi;
using Maggsoft.Core.Base;

namespace Newspaper.AdminPanel.Controllers
{
    public class TagsController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;
        private readonly ILogger<TagsController> _logger;

        public TagsController(IMaggsoftHttpClient httpClient, ILogger<TagsController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            try
            {
                var queryParams = new Dictionary<string, string>
                {
                    ["page"] = page.ToString(),
                    ["pageSize"] = pageSize.ToString()
                };

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    queryParams["searchTerm"] = searchTerm;
                }

                var url = "api/tags";
                if (queryParams.Any())
                {
                    var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                    url = $"{url}?{queryString}";
                }
                var result = await _httpClient.GetAsync<Result<PagedListWrapper<TagListDto>>>(url);

                if (result?.IsSuccess == true)
                {
                    var viewModel = new TagListViewModel
                    {
                        Tags = result.Data ?? PagedListWrapper<TagListDto>.Empty(page, pageSize),
                        SearchTerm = searchTerm
                    };

                    return View(viewModel);
                }

                var emptyViewModel = new TagListViewModel
                {
                    Tags = PagedListWrapper<TagListDto>.Empty(page, pageSize),
                    SearchTerm = searchTerm
                };

                TempData["ErrorMessage"] = "Etiket listesi alınırken bir hata oluştu.";
                return View(emptyViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket listesi alınırken hata oluştu");

                var emptyViewModel = new TagListViewModel
                {
                    Tags = PagedListWrapper<TagListDto>.Empty(page, pageSize),
                    SearchTerm = searchTerm
                };

                TempData["ErrorMessage"] = "Etiket listesi alınırken bir hata oluştu.";
                return View(emptyViewModel);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var createTagRequest = new
                {
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = model.IsActive
                };

                var result = await _httpClient.PostAsync<Result<TagDetailDto>>("api/tags", createTagRequest);

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Etiket başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result?.Message?.ToString() ?? "Etiket oluşturulurken bir hata oluştu.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket oluşturulurken hata oluştu");
                ModelState.AddModelError(string.Empty, "Etiket oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var result = await _httpClient.GetAsync<Result<TagDetailDto>>($"api/tags/{id}");

                if (result?.IsSuccess == true)
                {
                    var tag = result.Data;

                    if (tag != null)
                    {
                        var editModel = new EditTagViewModel
                        {
                            Id = tag.Id,
                            Name = tag.Name,
                            Description = tag.Description,
                            IsActive = tag.IsActive
                        };

                        return View(editModel);
                    }
                }

                TempData["ErrorMessage"] = "Etiket bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket düzenleme sayfası yüklenirken hata oluştu");
                TempData["ErrorMessage"] = "Etiket bilgileri alınırken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var updateTagRequest = new
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    IsActive = model.IsActive
                };

                var result = await _httpClient.PutAsync<Result<TagDetailDto>>($"api/tags/{model.Id}", updateTagRequest);

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Etiket başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result?.Message?.ToString() ?? "Etiket güncellenirken bir hata oluştu.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket güncellenirken hata oluştu");
                ModelState.AddModelError(string.Empty, "Etiket güncellenirken bir hata oluştu. Lütfen tekrar deneyin.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync("api/tags", id);

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Etiket başarıyla silindi.";
                }
                else
                {
                    TempData["ErrorMessage"] = result?.Message ?? "Etiket silinirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Etiket silinirken hata oluştu");
                TempData["ErrorMessage"] = "Etiket silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
