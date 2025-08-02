namespace Newspaper.Api.Services
{
    /// <summary>
    /// Migration Servisi Interface'i
    /// </summary>
    public interface IMigrationService
    {
        /// <summary>
        /// Migration'ları çalıştır
        /// </summary>
        Task MigrateAsync();

        /// <summary>
        /// Migration durumunu kontrol et
        /// </summary>
        Task<bool> HasMigrationsToApplyAsync();

        /// <summary>
        /// Migration listesini getir
        /// </summary>
        Task<IEnumerable<string>> GetMigrationListAsync();
    }
} 