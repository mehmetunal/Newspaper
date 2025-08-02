using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Newspaper.Data.Mssql;

namespace Newspaper.IdentityManager
{
    /// <summary>
    /// Denetlenebilir giriş yöneticisi
    /// </summary>
    public class AuditableSignInManager : SignInManager<User>
    {
        public AuditableSignInManager(
            UserManager<User> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        /// <summary>
        /// Giriş yapma işlemi (son giriş tarihini günceller)
        /// </summary>
        /// <param name="user">Kullanıcı</param>
        /// <param name="password">Şifre</param>
        /// <param name="isPersistent">Kalıcı oturum</param>
        /// <param name="lockoutOnFailure">Başarısızlıkta kilitle</param>
        /// <returns>Giriş sonucu</returns>
        public override async Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);

            if (result.Succeeded)
            {
                // Son giriş tarihini güncelle
                user.LastLoginDate = DateTime.UtcNow;
                await UserManager.UpdateAsync(user);
            }

            return result;
        }

        /// <summary>
        /// Giriş yapma işlemi (kullanıcı adı ile)
        /// </summary>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="password">Şifre</param>
        /// <param name="isPersistent">Kalıcı oturum</param>
        /// <param name="lockoutOnFailure">Başarısızlıkta kilitle</param>
        /// <returns>Giriş sonucu</returns>
        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (user != null)
                {
                    // Son giriş tarihini güncelle
                    user.LastLoginDate = DateTime.UtcNow;
                    await UserManager.UpdateAsync(user);
                }
            }

            return result;
        }
    }
} 