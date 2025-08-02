using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Newspaper.Data.Mssql;

namespace Newspaper.IdentityManager
{
    /// <summary>
    /// Özel claims principal factory
    /// </summary>
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        /// <summary>
        /// Claims oluşturur
        /// </summary>
        /// <param name="user">Kullanıcı</param>
        /// <returns>Claims principal</returns>
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);

            // Özel claims ekle
            var identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                // Kullanıcı ID'si
                identity.AddClaim(new Claim("UserId", user.Id.ToString()));

                // Tam ad
                identity.AddClaim(new Claim("FullName", user.FullName));

                // Ad
                identity.AddClaim(new Claim("FirstName", user.FirstName));

                // Soyad
                identity.AddClaim(new Claim("LastName", user.LastName));

                // Email
                if (!string.IsNullOrEmpty(user.Email))
                {
                    identity.AddClaim(new Claim("Email", user.Email));
                }

                // Profil resmi
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    identity.AddClaim(new Claim("ProfileImageUrl", user.ProfileImageUrl));
                }

                // Cinsiyet
                identity.AddClaim(new Claim("Gender", user.Gender.ToString()));

                // Son giriş tarihi
                if (user.LastLoginDate.HasValue)
                {
                    identity.AddClaim(new Claim("LastLoginDate", user.LastLoginDate.Value.ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }

            return principal;
        }
    }
} 