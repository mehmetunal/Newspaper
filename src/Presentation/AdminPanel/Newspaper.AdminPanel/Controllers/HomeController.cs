using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Services;
using Newspaper.AdminPanel.Models;
using Newspaper.Dto.Mssql;
using Maggsoft.Framework.HttpClientApi;
using Maggsoft.Core.Base;
using Microsoft.AspNetCore.Authorization;

namespace Newspaper.AdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor,Author,Moderator")]
    public class HomeController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMaggsoftHttpClient httpClient, ILogger<HomeController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardViewModel = new DashboardViewModel();

                // API'den istatistikleri çek
                var usersResult = await _httpClient.GetAsync<Result<string>>("api/users/count");
                var articlesResult = await _httpClient.GetAsync<Result<string>>("api/articles/count");
                var categoriesResult = await _httpClient.GetAsync<Result<string>>("api/categories/count");
                var commentsResult = await _httpClient.GetAsync<Result<string>>("api/comments/count");

                // Son haberleri çek
                var recentArticlesResult = await _httpClient.GetAsync<Result<List<ArticleListDto>>>("api/articles/recent");

                // Son yorumları çek
                var recentCommentsResult = await _httpClient.GetAsync<Result<List<CommentListDto>>>("api/comments/recent");

                // Son kullanıcıları çek
                var recentUsersResult = await _httpClient.GetAsync<Result<List<UserListDto>>>("api/users/recent");

                // Dashboard verilerini doldur
                dashboardViewModel.TotalUsers = int.TryParse(usersResult?.Data, out var userCount) ? userCount : 0;
                dashboardViewModel.TotalArticles = int.TryParse(articlesResult?.Data, out var articleCount) ? articleCount : 0;
                dashboardViewModel.TotalCategories = int.TryParse(categoriesResult?.Data, out var categoryCount) ? categoryCount : 0;
                dashboardViewModel.TotalComments = int.TryParse(commentsResult?.Data, out var commentCount) ? commentCount : 0;

                dashboardViewModel.RecentArticles = recentArticlesResult?.Data ?? new List<ArticleListDto>();
                dashboardViewModel.RecentComments = recentCommentsResult?.Data ?? new List<CommentListDto>();
                dashboardViewModel.RecentUsers = recentUsersResult?.Data ?? new List<UserListDto>();

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard verileri yüklenirken hata oluştu");
                TempData["Error"] = "Dashboard verileri yüklenirken bir hata oluştu.";
                return View(new DashboardViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
