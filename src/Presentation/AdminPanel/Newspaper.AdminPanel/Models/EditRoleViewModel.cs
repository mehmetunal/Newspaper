using System.ComponentModel.DataAnnotations;

namespace Newspaper.AdminPanel.Models
{
    public class EditRoleViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Rol adı zorunludur")]
        [StringLength(50, ErrorMessage = "Rol adı en fazla 50 karakter olabilir")]
        [Display(Name = "Rol Adı")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }
} 