using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Dashboard için ViewModel
    /// </summary>
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalArticles { get; set; }
        public int TotalComments { get; set; }
        public int TotalViews { get; set; }
        public int TotalCategories { get; set; }
        public int TotalTags { get; set; }
        public List<ArticleListDto> RecentArticles { get; set; } = new();
        public List<CommentListDto> RecentComments { get; set; } = new();
        public List<UserListDto> RecentUsers { get; set; } = new();
    }
} 