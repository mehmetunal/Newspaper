using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Kullanıcı listesi için ViewModel
    /// </summary>
    public class UserListViewModel
    {
        public PagedListWrapper<UserListDto> Users { get; set; } = PagedListWrapper<UserListDto>.Empty();
        public string SearchTerm { get; set; } = string.Empty;
    }
} 