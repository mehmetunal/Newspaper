namespace Newspaper.Dto.Mssql
{
    /// <summary>
    /// Tüm DTO'lar için temel sınıf
    /// </summary>
    public abstract class BaseDto
    {
        /// <summary>
        /// Benzersiz kimlik
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Güncellenme tarihi
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Yayın durumu
        /// </summary>
        public bool IsPublish { get; set; }
    }
} 