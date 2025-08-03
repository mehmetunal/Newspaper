namespace Newspaper.Dto.Mssql.Common
{
    /// <summary>
    /// Login işlemi sonrası dönen response DTO
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Kullanıcı Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcı rolü
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// Token geçerlilik süresi
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
} 

