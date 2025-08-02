using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql.Role
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "Rol adı zorunludur")]
        [StringLength(50, ErrorMessage = "Rol adı en fazla 50 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
