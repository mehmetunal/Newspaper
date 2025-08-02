namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Etiket oluşturma için ViewModel
    /// </summary>
    public class CreateTagViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
} 