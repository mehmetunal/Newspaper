using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Kategori listesi için ViewModel
    /// </summary>
    public class CategoryListViewModel
    {
        public PagedListWrapper<CategoryListDto> Categories { get; set; } = PagedListWrapper<CategoryListDto>.Empty();
        public string SearchTerm { get; set; } = string.Empty;
    }
} 