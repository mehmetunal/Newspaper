using Newspaper.Dto.Mssql;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.IoC;
using Newspaper.Dto.Mssql.Role;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Kullanıcı servis interface'i
    /// </summary>
    public interface IUserService : IService
    {
        /// <summary>
        /// Kullanıcı listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sayfalanmış kullanıcı listesi</returns>
        Task<PagedList<UserListDto>> GetUsersAsync(int page = 1, int pageSize = 10, string? searchTerm = null);

        /// <summary>
        /// Kullanıcı detayını getirir
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>Kullanıcı detayı</returns>
        Task<UserDetailDto?> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Kullanıcı oluşturur
        /// </summary>
        /// <param name="createUserDto">Kullanıcı oluşturma DTO'su</param>
        /// <returns>Oluşturulan kullanıcı</returns>
        Task<UserDetailDto> CreateUserAsync(CreateUserDto createUserDto);

        /// <summary>
        /// Kullanıcı günceller
        /// </summary>
        /// <param name="updateUserDto">Kullanıcı güncelleme DTO'su</param>
        /// <returns>Güncellenen kullanıcı</returns>
        Task<UserDetailDto> UpdateUserAsync(UpdateUserDto updateUserDto);

        /// <summary>
        /// Kullanıcı şifresini değiştirir
        /// </summary>
        /// <param name="changePasswordDto">Şifre değiştirme DTO'su</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        /// <summary>
        /// Kullanıcı siler (soft delete)
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> DeleteUserAsync(Guid id);

        /// <summary>
        /// Kullanıcıyı geri yükler
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> RestoreUserAsync(Guid id);

        // Rol yönetimi metodları
        /// <summary>
        /// Tüm rolleri getirir
        /// </summary>
        /// <returns>Rol listesi</returns>
        Task<List<RoleDto>> GetRolesAsync();

        /// <summary>
        /// Rol detayını getirir
        /// </summary>
        /// <param name="id">Rol ID</param>
        /// <returns>Rol detayı</returns>
        Task<RoleDto?> GetRoleByIdAsync(Guid id);

        /// <summary>
        /// Rol oluşturur
        /// </summary>
        /// <param name="createRoleDto">Rol oluşturma DTO'su</param>
        /// <returns>Oluşturulan rol</returns>
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);

        /// <summary>
        /// Rol günceller
        /// </summary>
        /// <param name="id">Rol ID</param>
        /// <param name="updateRoleDto">Rol güncelleme DTO'su</param>
        /// <returns>Güncellenen rol</returns>
        Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto updateRoleDto);

        /// <summary>
        /// Rol siler
        /// </summary>
        /// <param name="id">Rol ID</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> DeleteRoleAsync(Guid id);

        /// <summary>
        /// Kullanıcıya rol atar
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="roleName">Rol adı</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> AssignRoleToUserAsync(Guid userId, string roleName);

        /// <summary>
        /// Kullanıcıdan rol kaldırır
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="roleName">Rol adı</param>
        /// <returns>İşlem sonucu</returns>
        Task<bool> RemoveRoleFromUserAsync(Guid userId, string roleName);
    }
} 