using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Etiket listesi için ViewModel
    /// </summary>
    public class TagListViewModel
    {
        public PagedListWrapper<TagListDto> Tags { get; set; } = PagedListWrapper<TagListDto>.Empty();
        public string SearchTerm { get; set; } = string.Empty;
    }
} 