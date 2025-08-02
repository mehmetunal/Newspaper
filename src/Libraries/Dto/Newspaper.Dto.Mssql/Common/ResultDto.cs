namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Genel sonuç DTO'su
    /// </summary>
    /// <typeparam name="T">Veri tipi</typeparam>
    public class ResultDto<T>
    {
        /// <summary>
        /// Başarı durumu
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Mesaj
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Veri
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Hata kodu
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Başarılı sonuç oluşturur
        /// </summary>
        /// <param name="data">Veri</param>
        /// <param name="message">Mesaj</param>
        /// <returns>Başarılı sonuç</returns>
        public static ResultDto<T> SuccessResult(T data, string? message = null)
        {
            return new ResultDto<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// Hata sonucu oluşturur
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        /// <param name="errorCode">Hata kodu</param>
        /// <returns>Hata sonucu</returns>
        public static ResultDto<T> ErrorResult(string message, string? errorCode = null)
        {
            return new ResultDto<T>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
} 