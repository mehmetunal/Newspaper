using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Models;
using Maggsoft.Framework.HttpClientApi;
using Maggsoft.Core.Base;

namespace Newspaper.AdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor,Author,Moderator")]
    public class DashboardController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;

        public DashboardController(IMaggsoftHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard";

            try
            {
                var dashboardViewModel = new DashboardViewModel();

                // Kullanıcı sayısını al
                var usersResult = await _httpClient.GetAsync<Result<string>>("api/user/count");
                if (usersResult?.IsSuccess == true)
                {
                    dashboardViewModel.TotalUsers = int.TryParse(usersResult.Data, out var userCount) ? userCount : 0;
                }

                // Makale sayısını al
                var articlesResult = await _httpClient.GetAsync<Result<string>>("api/article/count");
                if (articlesResult?.IsSuccess == true)
                {
                    dashboardViewModel.TotalArticles = int.TryParse(articlesResult.Data, out var articleCount) ? articleCount : 0;
                }

                // Yorum sayısını al
                var commentsResult = await _httpClient.GetAsync<Result<string>>("api/comment/count");
                if (commentsResult?.IsSuccess == true)
                {
                    dashboardViewModel.TotalComments = int.TryParse(commentsResult.Data, out var commentCount) ? commentCount : 0;
                }

                // Kategori sayısını al
                var categoriesResult = await _httpClient.GetAsync<Result<string>>("api/category/count");
                if (categoriesResult?.IsSuccess == true)
                {
                    dashboardViewModel.TotalCategories = int.TryParse(categoriesResult.Data, out var categoryCount) ? categoryCount : 0;
                }

                // Tag sayısını al
                var tagsResult = await _httpClient.GetAsync<Result<string>>("api/tag/count");
                if (tagsResult?.IsSuccess == true)
                {
                    dashboardViewModel.TotalTags = int.TryParse(tagsResult.Data, out var tagCount) ? tagCount : 0;
                }

                // Görüntülenme sayısı (şimdilik 0)
                dashboardViewModel.TotalViews = 0;

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                // Hata durumunda varsayılan değerlerle devam et
                var dashboardViewModel = new DashboardViewModel
                {
                    TotalUsers = 0,
                    TotalArticles = 0,
                    TotalComments = 0,
                    TotalViews = 0,
                    TotalCategories = 0,
                    TotalTags = 0
                };

                TempData["Error"] = "Dashboard verileri yüklenirken hata oluştu: " + ex.Message;
                return View(dashboardViewModel);
            }
        }
    }
} 