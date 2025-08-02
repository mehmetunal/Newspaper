using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.Dto.Mssql;
using Newspaper.Dto.Mssql.Role;
using Newspaper.Mssql.Services;

namespace Newspaper.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var result = await userService.GetRolesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var result = await userService.GetRoleByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            var result = await userService.CreateRoleAsync(createRoleDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            var result = await userService.UpdateRoleAsync(id, updateRoleDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await userService.DeleteRoleAsync(id);
            return Ok(result);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto assignRoleDto)
        {
            var result = await userService.AssignRoleToUserAsync(assignRoleDto.UserId, assignRoleDto.RoleName);
            return Ok(result);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleDto removeRoleDto)
        {
            var result = await userService.RemoveRoleFromUserAsync(removeRoleDto.UserId, removeRoleDto.RoleName);
            return Ok(result);
        }
    }
}
