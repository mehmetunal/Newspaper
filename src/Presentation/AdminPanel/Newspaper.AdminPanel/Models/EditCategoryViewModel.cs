using System.ComponentModel.DataAnnotations;

namespace Newspaper.AdminPanel.Models
{
    public class EditCategoryViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Slug en fazla 100 karakter olabilir")]
        [Display(Name = "Slug")]
        public string? Slug { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }
}
