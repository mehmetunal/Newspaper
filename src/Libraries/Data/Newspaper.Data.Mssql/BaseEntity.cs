namespace Newspaper.Data.Mssql
{
    /// <summary>
    /// Tüm entity'ler için temel sınıf
    /// </summary>
    public abstract class BaseEntity : Maggsoft.Data.Mssql.BaseEntity
    {
        /// <summary>
        /// Geri yükleme işlemi
        /// </summary>
        public virtual void Restore()
        {
            IsPublish = true;
            IsDeleted = false;
            ModifiedDate = DateTime.UtcNow;
        }
    }
} 