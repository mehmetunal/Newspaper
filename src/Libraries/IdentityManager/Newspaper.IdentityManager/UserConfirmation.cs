using Microsoft.AspNetCore.Identity;
using Newspaper.Data.Mssql;

namespace Newspaper.IdentityManager
{
    /// <summary>
    /// Kullanıcı onay servisi
    /// </summary>
    public class UserConfirmation : IUserConfirmation<User>
    {
        /// <summary>
        /// Kullanıcının onaylanıp onaylanmadığını kontrol eder
        /// </summary>
        /// <param name="userManager">Kullanıcı yöneticisi</param>
        /// <param name="user">Kullanıcı</param>
        /// <returns>Onay durumu</returns>
        public async Task<bool> IsConfirmedAsync(UserManager<User> userManager, User user)
        {
            // Email onayı gerekli mi kontrol et
            if (userManager.Options.SignIn.RequireConfirmedEmail)
            {
                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    return false;
                }
            }

            // Telefon onayı gerekli mi kontrol et
            if (userManager.Options.SignIn.RequireConfirmedPhoneNumber)
            {
                if (!await userManager.IsPhoneNumberConfirmedAsync(user))
                {
                    return false;
                }
            }

            // Kullanıcı aktif mi kontrol et
            if (!user.EmailConfirmed) // Email onayı yapılmamışsa
            {
                return false;
            }

            return true;
        }
    }
} 