using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Maggsoft.Core.Base;
using Newspaper.Dto.Mssql;
using Newspaper.Mssql.Services;
using Newspaper.Data.Mssql;
using Newspaper.Dto.Mssql.Common;

namespace Newspaper.Api.Controllers
{
    /// <summary>
    /// Kimlik doğrulama işlemleri
    /// </summary>
    public class AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IUserService userService,
        ILogger<AuthController> logger)
        : BaseController
    {
        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        /// <param name="loginDto">Giriş bilgileri</param>
        /// <returns>Giriş sonucu</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<LoginResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return Error<LoginResponseDto>("Geçersiz email veya şifre", 401);
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    return Error<LoginResponseDto>("Geçersiz email veya şifre", 401);
                }

                // Son giriş tarihini güncelle
                user.LastLoginDate = DateTime.UtcNow;
                await userManager.UpdateAsync(user);

                // Kullanıcı bilgilerini al
                var roles = await userManager.GetRolesAsync(user);

                // Bearer Token oluştur
                var token = await userManager.GenerateUserTokenAsync(user, "Default", "BearerToken");

                var response = new LoginResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    RoleName = roles.FirstOrDefault() ?? "",
                    ExpiresAt = DateTime.UtcNow.AddDays(1)
                };

                logger.LogInformation("Kullanıcı giriş yaptı: {Email}", loginDto.Email);
                return Success(response, "Giriş başarılı");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Giriş işlemi sırasında hata oluştu");
                return Error<LoginResponseDto>("Giriş işlemi sırasında hata oluştu", 500);
            }
        }

        /// <summary>
        /// Kullanıcı kaydı
        /// </summary>
        /// <param name="registerDto">Kayıt bilgileri</param>
        /// <returns>Kayıt sonucu</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<UserDetailDto>>> Register([FromBody] CreateUserDto registerDto)
        {
            try
            {
                var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return Error<UserDetailDto>("Bu email adresi zaten kullanılıyor");
                }

                var user = new User
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    EmailConfirmed = true // Email onayı otomatik
                };

                var result = await userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return Error<UserDetailDto>($"Kayıt işlemi başarısız: {string.Join(", ", errors)}");
                }

                // Varsayılan olarak User rolü ata
                await userManager.AddToRoleAsync(user, "User");

                var userDetail = new UserDetailDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    RoleName = "User",
                    ProfileImageUrl = user.ProfileImageUrl,
                    Biography = user.Biography
                };

                logger.LogInformation("Yeni kullanıcı kaydı: {Email}", registerDto.Email);
                return Success(userDetail, "Kayıt başarılı");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kayıt işlemi sırasında hata oluştu");
                return Error<UserDetailDto>("Kayıt işlemi sırasında hata oluştu", 500);
            }
        }

        /// <summary>
        /// Kullanıcı çıkışı
        /// </summary>
        /// <returns>Çıkış sonucu</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<Result<object>>> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Success<object>(null, "Çıkış başarılı");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Çıkış işlemi sırasında hata oluştu");
                return Error<object>("Çıkış işlemi sırasında hata oluştu", 500);
            }
        }

        /// <summary>
        /// Mevcut kullanıcı bilgilerini getir
        /// </summary>
        /// <returns>Kullanıcı bilgileri</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<Result<UserDetailDto>>> GetCurrentUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Error<UserDetailDto>("Kullanıcı bilgisi bulunamadı", 401);
                }

                var user = await userService.GetUserByIdAsync(userId.Value);
                if (user == null)
                {
                    return Error<UserDetailDto>("Kullanıcı bulunamadı", 404);
                }

                return Success(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Kullanıcı bilgileri getirilirken hata oluştu");
                return Error<UserDetailDto>("Kullanıcı bilgileri getirilirken hata oluştu", 500);
            }
        }

        /// <summary>
        /// Şifre değiştirme
        /// </summary>
        /// <param name="changePasswordDto">Şifre değiştirme bilgileri</param>
        /// <returns>İşlem sonucu</returns>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<Result<object>>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return Error<object>("Kullanıcı bilgisi bulunamadı", 401);
                }

                var result = await userService.ChangePasswordAsync(changePasswordDto);
                if (!result)
                {
                    return Error<object>("Şifre değiştirme işlemi başarısız");
                }

                return Success<object>(null, "Şifre başarıyla değiştirildi");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Şifre değiştirme işlemi sırasında hata oluştu");
                return Error<object>("Şifre değiştirme işlemi sırasında hata oluştu", 500);
            }
        }

    }
}
