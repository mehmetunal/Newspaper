using Newspaper.Dto.Mssql;

namespace Newspaper.AdminPanel.Models
{
    /// <summary>
    /// Kullanıcı oluşturma için ViewModel
    /// </summary>
    public class CreateUserViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
} 