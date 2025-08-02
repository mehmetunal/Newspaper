namespace Newspaper.Dto.Mssql.Common;

    /// <summary>
    /// Giriş DTO'su
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Şifre
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
