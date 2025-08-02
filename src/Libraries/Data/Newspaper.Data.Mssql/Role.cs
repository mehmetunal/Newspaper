using Microsoft.AspNetCore.Identity;

namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Rol entity'si
    /// </summary>
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }
        /// <summary>
        /// Rol açıklaması
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Güncellenme tarihi
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Aktif/Pasif durumu
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Silinme durumu (soft delete)
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
