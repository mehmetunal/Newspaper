using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using System.Security.Claims;
using Maggsoft.Core.Model;

namespace Newspaper.Api.Controllers
{
    /// <summary>
    /// Tüm controller'lar için base controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Mevcut kullanıcının ID'sini alır
        /// </summary>
        /// <returns>Kullanıcı ID'si</returns>
        protected Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }

        /// <summary>
        /// Mevcut kullanıcının email'ini alır
        /// </summary>
        /// <returns>Kullanıcı email'i</returns>
        protected string? GetCurrentUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }

        /// <summary>
        /// Mevcut kullanıcının rollerini alır
        /// </summary>
        /// <returns>Kullanıcı rolleri</returns>
        protected IEnumerable<string> GetCurrentUserRoles()
        {
            return User.FindAll(ClaimTypes.Role).Select(c => c.Value);
        }

        /// <summary>
        /// Kullanıcının belirli bir role sahip olup olmadığını kontrol eder
        /// </summary>
        /// <param name="roleName">Rol adı</param>
        /// <returns>Rol sahibi mi?</returns>
        protected bool HasRole(string roleName)
        {
            return GetCurrentUserRoles().Contains(roleName);
        }

        /// <summary>
        /// Başarılı yanıt döner
        /// </summary>
        /// <typeparam name="T">Veri tipi</typeparam>
        /// <param name="data">Veri</param>
        /// <param name="message">Mesaj</param>
        /// <returns>Başarılı yanıt</returns>
        protected ActionResult<Result<T>> Success<T>(T data, string message = "İşlem başarılı") where T : class
        {
            return Ok(Result<T>.Success(data, new Maggsoft.
                Core.Model.SuccessMessage("200",message)));
        }

        /// <summary>
        /// Hata yanıtı döner
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        /// <param name="statusCode">HTTP status kodu</param>
        /// <returns>Hata yanıtı</returns>
        protected ActionResult<Result<T>> Error<T>(string message, int statusCode = 400) where T : class
        {
            return BadRequest(Result<T>.Failure(new Error(statusCode.ToString(),message)));
        }
    }


}
