using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Haber listesi için ViewModel
    /// </summary>
    public class ArticleListViewModel
    {
        public PagedListWrapper<ArticleListDto> Articles { get; set; } = PagedListWrapper<ArticleListDto>.Empty();
        public string SearchTerm { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
    }
}
