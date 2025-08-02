namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Dosya yükleme sonucu DTO'su
    /// </summary>
    public class FileUploadResultDto
    {
        /// <summary>
        /// Başarı durumu
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Dosya adı
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Dosya URL'i
        /// </summary>
        public string? FileUrl { get; set; }

        /// <summary>
        /// Dosya boyutu (byte)
        /// </summary>
        public long FileSize { get; set; } = 0;

        /// <summary>
        /// Dosya türü
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Hata mesajı
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
} 