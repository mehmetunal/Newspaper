namespace Newspaper.Dto.Mssql.Common
{
    /// <summary>
    /// Login işlemi sonrası dönen response DTO
    /// </summary>
    public class LoginResponseDto
    {
     /// <summary>
        /// Kullanıcı bilgileri
        /// </summary>
        public UserDetailDto User { get; set; } = new();
    }
} 

