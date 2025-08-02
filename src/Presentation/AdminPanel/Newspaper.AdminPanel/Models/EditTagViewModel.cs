namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Etiket düzenleme için ViewModel
    /// </summary>
    public class EditTagViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
