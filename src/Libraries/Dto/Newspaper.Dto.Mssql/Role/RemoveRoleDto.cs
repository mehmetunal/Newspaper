using System.ComponentModel.DataAnnotations;

namespace Newspaper.Dto.Mssql.Role
{
    public class RemoveRoleDto
    {
        [Required(ErrorMessage = "Kullanıcı ID zorunludur")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Rol adı zorunludur")]
        public string RoleName { get; set; } = string.Empty;
    }
}
