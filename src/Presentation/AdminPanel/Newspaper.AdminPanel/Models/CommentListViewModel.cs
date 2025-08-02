using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Yorum listesi için ViewModel
    /// </summary>
    public class CommentListViewModel
    {
        public PagedListWrapper<CommentListDto> Comments { get; set; } = PagedListWrapper<CommentListDto>.Empty();
        public string SearchTerm { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ArticleId { get; set; } = string.Empty;
    }
} 