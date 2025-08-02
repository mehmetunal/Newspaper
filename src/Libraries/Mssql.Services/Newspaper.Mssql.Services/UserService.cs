using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newspaper.Data.Mssql;
using Newspaper.Dto.Mssql;
using Maggsoft.Core.Model.Pagination;
using Maggsoft.Core.Model;
using Maggsoft.Core.Extensions;
using Newspaper.Dto.Mssql.Role;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// Kullanıcı servis implementasyonu
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcı listesini getirir
        /// </summary>
        /// <param name="page">Sayfa numarası</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Sayfalanmış kullanıcı listesi</returns>
        public async Task<PagedList<UserListDto>> GetUsersAsync(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            try
            {
                var query = _userManager.Users.AsQueryable();

                // Arama filtresi
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(u =>
                        u.FirstName.Contains(searchTerm) ||
                        u.LastName.Contains(searchTerm) ||
                        u.Email!.Contains(searchTerm) ||
                        u.UserName!.Contains(searchTerm));
                }

                // UserListDto'ya dönüştür
                var userQuery = query.Select(u => new UserListDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email!,
                    FullName = $"{u.FirstName} {u.LastName}",
                    RoleName = "", // @TODO:  UserManager ile role bilgisi ayrıca alınmalı
                    ProfileImageUrl = u.ProfileImageUrl,
                    LastLoginDate = u.LastLoginDate,
                    ArticleCount = 0, // @TODO:  Ayrıca hesaplanmalı
                    CommentCount = 0 // @TODO:  Ayrıca hesaplanmalı
                });

                // PagedList kullan
                return await userQuery.ToPagedListAsync(page - 1, pageSize, new List<Filter>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı listesi getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Kullanıcı detayını getirir
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>Kullanıcı detayı</returns>
        public async Task<UserDetailDto?> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return null;

                var roles = await _userManager.GetRolesAsync(user);

                return new UserDetailDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    RoleName = roles.FirstOrDefault() ?? "",
                    FullName = $"{user.FirstName} {user.LastName}",
                    ProfileImageUrl = user.ProfileImageUrl,
                    Biography = user.Biography,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    LastLoginDate = user.LastLoginDate,
                    ArticleCount = 0, // @TODO: Ayrıca hesaplanmalı
                    CommentCount = 0, // @TODO: Ayrıca hesaplanmalı
                    Roles = roles.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı detayı getirilirken hata oluştu. ID: {UserId}", id);
                throw;
            }
        }

        /// <summary>
        /// Kullanıcı oluşturur
        /// </summary>
        /// <param name="createUserDto">Kullanıcı oluşturma DTO'su</param>
        /// <returns>Oluşturulan kullanıcı</returns>
        public async Task<UserDetailDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var user = new User
                {
                    UserName = createUserDto.UserName,
                    Email = createUserDto.Email,
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    ProfileImageUrl = createUserDto.ProfileImageUrl,
                    Biography = createUserDto.Biography,
                    BirthDate = createUserDto.BirthDate,
                    Gender = createUserDto.Gender,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, createUserDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Kullanıcı oluşturulamadı: {errors}");
                }

                // Rolleri ekle
                if (createUserDto.Roles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, createUserDto.Roles);
                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Roller eklenemedi: {errors}");
                    }
                }

                return await GetUserByIdAsync(user.Id) ?? throw new InvalidOperationException("Oluşturulan kullanıcı bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturulurken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Kullanıcı günceller
        /// </summary>
        /// <param name="updateUserDto">Kullanıcı güncelleme DTO'su</param>
        /// <returns>Güncellenen kullanıcı</returns>
        public async Task<UserDetailDto> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(updateUserDto.Id.ToString());
                if (user == null)
                    throw new InvalidOperationException("Kullanıcı bulunamadı");

                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.Email = updateUserDto.Email;
                user.UserName = updateUserDto.Email; // Email'i username olarak kullan
                user.ProfileImageUrl = updateUserDto.ProfileImageUrl;
                user.Biography = updateUserDto.Biography;
                user.BirthDate = updateUserDto.BirthDate;
                user.Gender = updateUserDto.Gender;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Kullanıcı güncellenemedi: {errors}");
                }

                return await GetUserByIdAsync(user.Id) ?? throw new InvalidOperationException("Güncellenen kullanıcı bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu. ID: {UserId}", updateUserDto.Id);
                throw;
            }
        }

        /// <summary>
        /// Şifre değiştirir
        /// </summary>
        /// <param name="changePasswordDto">Şifre değiştirme DTO'su</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(changePasswordDto.UserId.ToString());
                if (user == null)
                    return false;

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre değiştirilirken hata oluştu. UserId: {UserId}", changePasswordDto.UserId);
                return false;
            }
        }

        /// <summary>
        /// Kullanıcı siler (hard delete - IdentityUser soft delete desteklemiyor)
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return false;

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken hata oluştu. ID: {UserId}", id);
                return false;
            }
        }

        /// <summary>
        /// Kullanıcıyı geri yükler (IdentityUser soft delete desteklemiyor)
        /// </summary>
        /// <param name="id">Kullanıcı ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> RestoreUserAsync(Guid id)
        {
            try
            {
                //  @TODO:  IdentityUser soft delete desteklemiyor, bu method her zaman false döner
                _logger.LogWarning("IdentityUser soft delete desteklemiyor. RestoreUserAsync method'u kullanılamaz.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı geri yüklenirken hata oluştu. ID: {UserId}", id);
                return false;
            }
        }

        // Rol yönetimi metodları
        /// <summary>
        /// Tüm rolleri getirir
        /// </summary>
        /// <returns>Rol listesi</returns>
        public async Task<List<RoleDto>> GetRolesAsync()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                var roleDtos = new List<RoleDto>();

                foreach (var role in roles)
                {
                    var userCount = await _userManager.GetUsersInRoleAsync(role.Name!);
                    roleDtos.Add(new RoleDto
                    {
                        Id = role.Id,
                        Name = role.Name!,
                        Description = role.Description,
                        IsActive = role.IsActive,
                        UserCount = userCount.Count,
                        CreatedDate = role.CreatedAt,
                        ModifiedDate = role.UpdatedAt
                    });
                }

                return roleDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Roller getirilirken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Rol detayını getirir
        /// </summary>
        /// <param name="id">Rol ID</param>
        /// <returns>Rol detayı</returns>
        public async Task<RoleDto?> GetRoleByIdAsync(Guid id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                    return null;

                var userCount = await _userManager.GetUsersInRoleAsync(role.Name!);

                return new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name!,
                    Description = role.Description,
                    IsActive = role.IsActive,
                    UserCount = userCount.Count,
                    CreatedDate = role.CreatedAt,
                    ModifiedDate = role.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol detayı getirilirken hata oluştu. ID: {RoleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Rol oluşturur
        /// </summary>
        /// <param name="createRoleDto">Rol oluşturma DTO'su</param>
        /// <returns>Oluşturulan rol</returns>
        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            try
            {
                var role = new Role(createRoleDto.Name)
                {
                    Description = createRoleDto.Description,
                    IsActive = true
                };

                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Rol oluşturulamadı: {errors}");
                }

                return await GetRoleByIdAsync(role.Id) ?? throw new InvalidOperationException("Oluşturulan rol bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol oluşturulurken hata oluştu");
                throw;
            }
        }

        /// <summary>
        /// Rol günceller
        /// </summary>
        /// <param name="id">Rol ID</param>
        /// <param name="updateRoleDto">Rol güncelleme DTO'su</param>
        /// <returns>Güncellenen rol</returns>
        public async Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                    throw new InvalidOperationException("Rol bulunamadı");

                role.Name = updateRoleDto.Name;
                role.Description = updateRoleDto.Description;
                role.IsActive = updateRoleDto.IsActive;
                role.UpdatedAt = DateTime.UtcNow;

                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Rol güncellenemedi: {errors}");
                }

                return await GetRoleByIdAsync(role.Id) ?? throw new InvalidOperationException("Güncellenen rol bulunamadı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol güncellenirken hata oluştu. ID: {RoleId}", id);
                throw;
            }
        }

        /// <summary>
        /// Rol siler
        /// </summary>
        /// <param name="id">Rol ID</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                    return false;

                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol silinirken hata oluştu. ID: {RoleId}", id);
                return false;
            }
        }

        /// <summary>
        /// Kullanıcıya rol atar
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="roleName">Rol adı</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> AssignRoleToUserAsync(Guid userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcıya rol atanırken hata oluştu. UserId: {UserId}, Role: {RoleName}", userId, roleName);
                return false;
            }
        }

        /// <summary>
        /// Kullanıcıdan rol kaldırır
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="roleName">Rol adı</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcıdan rol kaldırılırken hata oluştu. UserId: {UserId}, Role: {RoleName}", userId, roleName);
                return false;
            }
        }
    }
}
