using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Models;
using Maggsoft.Core.Base;
using Maggsoft.Framework.HttpClientApi;
using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IMaggsoftHttpClient httpClient, ILogger<ArticlesController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string searchTerm = "", string categoryId = "")
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

                if (!string.IsNullOrEmpty(categoryId))
                {
                    queryParams["categoryId"] = categoryId;
                }

                var url = "api/articles";
                if (queryParams.Any())
                {
                    var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                    url = $"{url}?{queryString}";
                }
                var result = await _httpClient.GetAsync<Result<PagedListWrapper<ArticleListDto>>>(url);

                if (result?.IsSuccess == true)
                {
                    var viewModel = new ArticleListViewModel
                    {
                        Articles = result.Data ?? PagedListWrapper<ArticleListDto>.Empty(page, pageSize),
                        SearchTerm = searchTerm,
                        CategoryId = categoryId
                    };

                    return View(viewModel);
                }

                var emptyViewModel = new ArticleListViewModel
                {
                    Articles = PagedListWrapper<ArticleListDto>.Empty(page, pageSize),
                    SearchTerm = searchTerm,
                    CategoryId = categoryId
                };

                TempData["ErrorMessage"] = "Haber listesi alınırken bir hata oluştu.";
                return View(emptyViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber listesi alınırken hata oluştu");

                var emptyViewModel = new ArticleListViewModel
                {
                    Articles = PagedListWrapper<ArticleListDto>.Empty(page, pageSize),
                    SearchTerm = searchTerm,
                    CategoryId = categoryId
                };

                TempData["ErrorMessage"] = "Haber listesi alınırken bir hata oluştu.";
                return View(emptyViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                // Kategorileri al
                var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");

                var viewModel = new CreateArticleViewModel
                {
                    Categories = categoriesResponse ?? new List<CategoryListDto>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber oluşturma sayfası yüklenirken hata oluştu");
                TempData["ErrorMessage"] = "Kategoriler alınırken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Kategorileri tekrar yükle
                try
                {
                    var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");
                    model.Categories = categoriesResponse ?? new List<CategoryListDto>();
                }
                catch
                {
                    model.Categories = new List<CategoryListDto>();
                }

                return View(model);
            }

            try
            {
                var createArticleRequest = new
                {
                    Title = model.Title,
                    Content = model.Content,
                    Summary = model.Summary,
                    CategoryId = model.CategoryId,
                    IsPublished = model.IsPublished,
                    Tags = model.Tags?.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList() ?? new List<string>()
                };

                var result = await _httpClient.PostAsync<Result<ArticleDetailDto>>("api/articles", createArticleRequest);

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Haber başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result?.Message?.ToString() ?? "Haber oluşturulurken bir hata oluştu.");

                // Kategorileri tekrar yükle
                try
                {
                    var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");
                    model.Categories = categoriesResponse ?? new List<CategoryListDto>();
                }
                catch
                {
                    model.Categories = new List<CategoryListDto>();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber oluşturulurken hata oluştu");
                ModelState.AddModelError(string.Empty, "Haber oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");

                // Kategorileri tekrar yükle
                try
                {
                    var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");
                    model.Categories = categoriesResponse ?? new List<CategoryListDto>();
                }
                catch
                {
                    model.Categories = new List<CategoryListDto>();
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var result = await _httpClient.GetAsync<Result<ArticleDetailDto>>($"api/articles/{id}");

                if (result?.IsSuccess == true)
                {
                    // Kategorileri al
                    var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");

                    var editModel = new EditArticleViewModel
                    {
                        Id = result.Data.Id,
                        Title = result.Data.Title,
                        Content = result.Data.Content,
                        Summary = result.Data.Summary,
                        CategoryId = result.Data.CategoryId,
                        IsPublished = result.Data.IsPublished,
                        Tags = string.Join(", ", result.Data.Tags ?? new List<string>()),
                        Categories = categoriesResponse ?? new List<CategoryListDto>()
                    };

                    return View(editModel);
                }

                TempData["ErrorMessage"] = "Haber bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber düzenleme sayfası yüklenirken hata oluştu");
                TempData["ErrorMessage"] = "Haber bilgileri alınırken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditArticleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Kategorileri tekrar yükle
                try
                {
                    var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");
                    model.Categories = categoriesResponse ?? new List<CategoryListDto>();
                }
                catch
                {
                    model.Categories = new List<CategoryListDto>();
                }

                return View(model);
            }

            try
            {
                var updateArticleRequest = new
                {
                    Id = model.Id,
                    Title = model.Title,
                    Content = model.Content,
                    Summary = model.Summary,
                    CategoryId = model.CategoryId,
                    IsPublished = model.IsPublished,
                    Tags = model.Tags?.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList() ?? new List<string>()
                };

                var result = await _httpClient.PutAsync<Result<ArticleDetailDto>>($"api/articles/{model.Id}", updateArticleRequest);

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Haber başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result?.Message?.ToString() ?? "Haber güncellenirken bir hata oluştu.");

                // Kategorileri tekrar yükle
                try
                {
                    var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");
                    model.Categories = categoriesResponse ?? new List<CategoryListDto>();
                }
                catch
                {
                    model.Categories = new List<CategoryListDto>();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber güncellenirken hata oluştu");
                ModelState.AddModelError(string.Empty, "Haber güncellenirken bir hata oluştu. Lütfen tekrar deneyin.");

                // Kategorileri tekrar yükle
                try
                {
                    var categoriesResponse = await _httpClient.GetAllAsync<CategoryListDto>("api/categories");
                    model.Categories = categoriesResponse ?? new List<CategoryListDto>();
                }
                catch
                {
                    model.Categories = new List<CategoryListDto>();
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync("api/articles", id);

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Haber başarıyla silindi.";
                }
                else
                {
                    TempData["ErrorMessage"] = result?.Message ?? "Haber silinirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber silinirken hata oluştu");
                TempData["ErrorMessage"] = "Haber silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
