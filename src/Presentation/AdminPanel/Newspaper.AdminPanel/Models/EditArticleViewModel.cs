using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Haber düzenleme için ViewModel
    /// </summary>
    public class EditArticleViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public bool IsPublished { get; set; } = false;
        public string Tags { get; set; } = string.Empty;
        public List<CategoryListDto> Categories { get; set; } = new();
    }
}
