using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Yorum detayı için ViewModel
    /// </summary>
    public class CommentDetailViewModel
    {
        public CommentDetailDto Comment { get; set; } = new();
    }
} 