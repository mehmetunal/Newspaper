using Maggsoft.Core.Base;
using Maggsoft.Core.Model;
using Newspaper.Dto.Mssql;
using Newspaper.Dto.Mssql.Role;
using Newspaper.Mssql.Services;
using Microsoft.AspNetCore.Mvc;
using Maggsoft.Core.Model.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace Newspaper.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(
        IUserService userService,
        ILogger<UserController> logger)
        : BaseController
    {
        /// <summary>
        /// Kullanıcıları listeler (sayfalama ile)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin,Moderator")]
        public async Task<ActionResult<Result<PagedList<UserListDto>>>> GetUsers(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await userService.GetUsersAsync(pageNumber, pageSize, searchTerm);
                return Ok(Result<PagedList<UserListDto>>.Success(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcılar listelenirken hata oluştu");
                return StatusCode(500, Result<PagedList<UserListDto>>.Failure(new Error("500", "Kullanıcılar listelenirken hata oluştu")));
            }
        }

        /// <summary>
        /// Kullanıcı detayını getirir
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin,Moderator")]
        public async Task<ActionResult<Result<UserDetailDto>>> GetUser(Guid id)
        {
            try
            {
                var result = await userService.GetUserByIdAsync(id);
                if (result == null)
                {
                    return NotFound(Result<UserDetailDto>.Failure(new Error("404", "Kullanıcı bulunamadı")));
                }

                return Ok(Result<UserDetailDto>.Success(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcı detayı alınırken hata oluştu. ID: {UserId}", id);
                return StatusCode(500, Result<UserDetailDto>.Failure(new Error("500", "Kullanıcı detayı alınırken hata oluştu")));
            }
        }

        /// <summary>
        /// Yeni kullanıcı oluşturur
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Result<UserDetailDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var result = await userService.CreateUserAsync(createUserDto);
                return Ok(Result<UserDetailDto>.Success(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcı oluşturulurken hata oluştu");
                return StatusCode(500, Result<UserDetailDto>.Failure(new Error("500", "Kullanıcı oluşturulurken hata oluştu")));
            }
        }

        /// <summary>
        /// Kullanıcı bilgilerini günceller
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Result<UserDetailDto>>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                updateUserDto.Id = id;
                var result = await userService.UpdateUserAsync(updateUserDto);
                if (result == null)
                {
                    return NotFound(Result<UserDetailDto>.Failure(new Error("404", "Kullanıcı bulunamadı")));
                }

                return Ok(Result<UserDetailDto>.Success(result));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu. ID: {UserId}", id);
                return StatusCode(500, Result<UserDetailDto>.Failure(new Error("500", "Kullanıcı güncellenirken hata oluştu")));
            }
        }

        /// <summary>
        /// Kullanıcıyı siler (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Result>> DeleteUser(Guid id)
        {
            try
            {
                var result = await userService.DeleteUserAsync(id);
                if (!result)
                {
                    return NotFound(Result.Failure(new Error("404", "Kullanıcı bulunamadı")));
                }

                return Ok(Result.Success());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcı silinirken hata oluştu. ID: {UserId}", id);
                return StatusCode(500, Result.Failure(new Error("500", "Kullanıcı silinirken hata oluştu")));
            }
        }

        /// <summary>
        /// Kullanıcıyı geri yükler (soft delete'den)
        /// </summary>
        [HttpPost("{id}/restore")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Result>> RestoreUser(Guid id)
        {
            try
            {
                var result = await userService.RestoreUserAsync(id);
                if (!result)
                {
                    return NotFound(Result.Failure(new Error("404", "Kullanıcı bulunamadı")));
                }

                return Ok(Result.Success());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcı geri yüklenirken hata oluştu. ID: {UserId}", id);
                return StatusCode(500, Result.Failure(new Error("500", "Kullanıcı geri yüklenirken hata oluştu")));
            }
        }

        /// <summary>
        /// Kullanıcıya rol atar
        /// </summary>
        [HttpPost("{id}/assign-role")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Result>> AssignRoleToUser(Guid id, [FromBody] AssignRoleDto assignRoleDto)
        {
            try
            {
                assignRoleDto.UserId = id;
                var result = await userService.AssignRoleToUserAsync(id,assignRoleDto.RoleName);
                if (!result)
                {
                    return BadRequest(Result.Failure(new Error("400", "Rol atanamadı")));
                }

                return Ok(Result.Success());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcıya rol atanırken hata oluştu. UserID: {UserId}, Role: {Role}", id, assignRoleDto.RoleName);
                return StatusCode(500, Result.Failure(new Error("500", "Rol atanırken hata oluştu")));
            }
        }

        /// <summary>
        /// Kullanıcıdan rol kaldırır
        /// </summary>
        [HttpPost("{id}/remove-role")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<ActionResult<Result>> RemoveRoleFromUser(Guid id, [FromBody] RemoveRoleDto removeRoleDto)
        {
            try
            {
                removeRoleDto.UserId = id;
                var result = await userService.RemoveRoleFromUserAsync(id, removeRoleDto.RoleName);
                if (!result)
                {
                    return BadRequest(Result.Failure(new Error("400", "Rol kaldırılamadı")));
                }

                return Ok(Result.Success());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcıdan rol kaldırılırken hata oluştu. UserID: {UserId}, Role: {Role}", id, removeRoleDto.RoleName);
                return StatusCode(500, Result.Failure(new Error("500", "Rol kaldırılırken hata oluştu")));
            }
        }
    }
}
