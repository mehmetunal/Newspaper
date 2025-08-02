using FluentMigrator.Runner;

namespace Newspaper.Api.Services
{
    /// <summary>
    /// Migration Servisi
    /// </summary>
    public class MigrationService(IServiceProvider serviceProvider, ILogger<MigrationService> logger) 
    : IMigrationService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<MigrationService> _logger = logger;


        /// <summary>
        /// Migration'ları çalıştır
        /// </summary>
        public async Task MigrateAsync()
        {
            try
            {
                _logger.LogInformation("Database migration'ları başlatılıyor...");

                using var scope = _serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                if (runner.HasMigrationsToApplyUp())
                {
                    runner.MigrateUp();
                    _logger.LogInformation("Database migration'ları başarıyla tamamlandı.");
                }
                else
                {
                    _logger.LogInformation("Uygulanacak migration bulunamadı.");
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database migration sırasında hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Migration durumunu kontrol et
        /// </summary>
        public async Task<bool> HasMigrationsToApplyAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                return await Task.FromResult(runner.HasMigrationsToApplyUp());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Migration durumu kontrol edilirken hata oluştu.");
                return false;
            }
        }

        /// <summary>
        /// Migration listesini getir
        /// </summary>
        public async Task<IEnumerable<string>> GetMigrationListAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                var migrations = runner.MigrationLoader.LoadMigrations();
                
                return await Task.FromResult(migrations.Select(m => m.Key.ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Migration listesi alınırken hata oluştu.");
                return Enumerable.Empty<string>();
            }
        }
    }
} 