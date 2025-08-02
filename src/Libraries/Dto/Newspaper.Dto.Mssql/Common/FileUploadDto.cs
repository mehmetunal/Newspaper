using Microsoft.AspNetCore.Http;

namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Dosya yükleme DTO'su
    /// </summary>
    public class FileUploadDto
    {
        /// <summary>
        /// Yüklenecek dosya
        /// </summary>
        public IFormFile? File { get; set; }

        /// <summary>
        /// Dosya türü (image, document, etc.)
        /// </summary>
        public string? FileType { get; set; }

        /// <summary>
        /// Maksimum dosya boyutu (MB)
        /// </summary>
        public int MaxFileSizeInMB { get; set; } = 10;

        /// <summary>
        /// İzin verilen dosya uzantıları
        /// </summary>
        public string[]? AllowedExtensions { get; set; }
    }

    /// <summary>
    /// Resim yükleme DTO'su
    /// </summary>
    public class ImageUploadDto
    {
        /// <summary>
        /// Yüklenecek resim dosyası
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Maksimum dosya boyutu (MB)
        /// </summary>
        public int MaxFileSizeInMB { get; set; } = 5;

        /// <summary>
        /// İzin verilen resim formatları
        /// </summary>
        public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    }

    /// <summary>
    /// Profil resmi yükleme DTO'su
    /// </summary>
    public class ProfileImageUploadDto
    {
        /// <summary>
        /// Yüklenecek profil resmi
        /// </summary>
        public IFormFile? ProfileImage { get; set; }
    }

    /// <summary>
    /// Makale resmi yükleme DTO'su
    /// </summary>
    public class ArticleImageUploadDto
    {
        /// <summary>
        /// Yüklenecek makale resmi
        /// </summary>
        public IFormFile? ArticleImage { get; set; }

        /// <summary>
        /// Makale ID
        /// </summary>
        public Guid ArticleId { get; set; }
    }
} 