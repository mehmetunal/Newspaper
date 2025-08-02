using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Haber oluşturma için ViewModel
    /// </summary>
    public class CreateArticleViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public bool IsPublished { get; set; } = false;
        public string Tags { get; set; } = string.Empty;
        public List<CategoryListDto> Categories { get; set; } = new();
    }
}
