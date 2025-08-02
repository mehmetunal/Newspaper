using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Models;
using Newspaper.AdminPanel.Services;
using Newspaper.Dto.Mssql;
using Maggsoft.Framework.HttpClientApi;
using Maggsoft.Core.Base;
using System.Text.Json;

namespace Newspaper.AdminPanel.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(IMaggsoftHttpClient httpClient, ILogger<CommentsController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string searchTerm = "", string status = "")
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

                if (!string.IsNullOrEmpty(status))
                {
                    queryParams["status"] = status;
                }

                var url = "api/comments";
                if (queryParams.Any())
                {
                    var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                    url = $"{url}?{queryString}";
                }
                var result = await _httpClient.GetAsync<Result<PagedListWrapper<CommentListDto>>>(url);

                if (result?.IsSuccess == true)
                {
                    return View(new CommentListViewModel
                    {
                        Comments = result.Data ?? PagedListWrapper<CommentListDto>.Empty(page, pageSize),
                        SearchTerm = searchTerm,
                        Status = status
                    });
                }

                TempData["ErrorMessage"] = "Yorum listesi alınırken bir hata oluştu.";

                return View(new CommentListViewModel
                {
                    Comments = PagedListWrapper<CommentListDto>.Empty(page, pageSize),
                    SearchTerm = searchTerm,
                    Status = status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum listesi alınırken hata oluştu");

                TempData["ErrorMessage"] = "Yorum listesi alınırken bir hata oluştu.";
                return View(new CommentListViewModel
                {
                    Comments = PagedListWrapper<CommentListDto>.Empty(page, pageSize),
                    SearchTerm = searchTerm,
                    Status = status
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var result = await _httpClient.GetAsync<Result<CommentDetailDto>>($"api/comments/{id}");

                if (result?.IsSuccess == true)
                {
                    return View(new CommentDetailViewModel
                    {
                        Comment = result.Data
                    });
                }

                TempData["ErrorMessage"] = "Yorum bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum detay sayfası yüklenirken hata oluştu");
                TempData["ErrorMessage"] = "Yorum bilgileri alınırken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string id)
        {
            try
            {
                var result = await _httpClient.PutAsync<Result<CommentDetailDto>>($"api/comments/{id}/approve", new { });

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Yorum başarıyla onaylandı.";
                }
                else
                {
                    TempData["ErrorMessage"] = result?.Message ?? "Yorum onaylanırken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum onaylanırken hata oluştu");
                TempData["ErrorMessage"] = "Yorum onaylanırken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(string id)
        {
            try
            {
                var result = await _httpClient.PutAsync<Result<CommentDetailDto>>($"api/comments/{id}/reject", new { });

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Yorum başarıyla reddedildi.";
                }
                else
                {
                    TempData["ErrorMessage"] = result?.Message ?? "Yorum reddedilirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum reddedilirken hata oluştu");
                TempData["ErrorMessage"] = "Yorum reddedilirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync("api/comments", id);

                if (result?.IsSuccess == true)
                {
                    TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
                }
                else
                {
                    TempData["ErrorMessage"] = result?.Message ?? "Yorum silinirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorum silinirken hata oluştu");
                TempData["ErrorMessage"] = "Yorum silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ByArticle(string articleId, int page = 1, int pageSize = 10)
        {
            try
            {
                var queryParams = new Dictionary<string, string>
                {
                    ["page"] = page.ToString(),
                    ["pageSize"] = pageSize.ToString()
                };

                var url = $"api/comments/article/{articleId}";
                if (queryParams.Any())
                {
                    var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                    url = $"{url}?{queryString}";
                }
                var result = await _httpClient.GetAsync<Result<PagedListWrapper<CommentListDto>>>(url);

                if (result?.IsSuccess == true)
                {
                    return View("Index", new CommentListViewModel
                    {
                        Comments = result.Data ?? PagedListWrapper<CommentListDto>.Empty(page, pageSize),
                        ArticleId = articleId
                    });
                }

                TempData["ErrorMessage"] = "Haber yorumları alınırken bir hata oluştu.";

                return View("Index", new CommentListViewModel
                {
                    Comments = PagedListWrapper<CommentListDto>.Empty(page, pageSize),
                    ArticleId = articleId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Haber yorumları alınırken hata oluştu");

                TempData["ErrorMessage"] = "Haber yorumları alınırken bir hata oluştu.";
                
                return View("Index", new CommentListViewModel
                {
                    Comments = PagedListWrapper<CommentListDto>.Empty(page, pageSize),
                    ArticleId = articleId
                });
            }
        }
    }
}
