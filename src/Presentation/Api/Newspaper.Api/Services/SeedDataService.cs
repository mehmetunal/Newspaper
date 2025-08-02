using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data.Mssql;
using Newspaper.Mssql;

namespace Newspaper.Api.Services
{
    /// <summary>
    /// Seed Data Servisi
    /// </summary>
    public class SeedDataService : ISeedDataService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SeedDataService> _logger;

        public SeedDataService(IServiceProvider serviceProvider, ILogger<SeedDataService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Tüm seed data'yı oluştur
        /// </summary>
        public async Task SeedAllDataAsync()
        {
            try
            {
                _logger.LogInformation("Seed data oluşturma işlemi başlatılıyor...");

                await CreateRolesAsync();
                await CreateAdminUserAsync();
                await CreateCategoriesAsync();
                await CreateTagsAsync();
                await CreateArticlesAsync();
                await CreateCommentsAsync();

                _logger.LogInformation("Seed data oluşturma işlemi başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seed data oluşturma sırasında hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Rolleri oluştur
        /// </summary>
        public async Task CreateRolesAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var roles = new[]
                {
                    new { Name = "SuperAdmin", Description = "Sistemin tam kontrolüne sahip en üst seviye rol" },
                    new { Name = "Admin", Description = "Genel sistem yönetimi ve içerik kontrolü" },
                    new { Name = "Editor", Description = "İçerik yönetimi ve yayınlama" },
                    new { Name = "Author", Description = "Makale yazma ve kendi içeriklerini yönetme" },
                    new { Name = "Moderator", Description = "Yorum moderasyonu ve kullanıcı yönetimi" },
                    new { Name = "Viewer", Description = "Sadece içerik görüntüleme" }
                };

                var createdRoles = 0;

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        var newRole = new Role(role.Name)
                        {
                            Description = role.Description,
                            IsActive = true
                        };
                        await roleManager.CreateAsync(newRole);
                        createdRoles++;
                        _logger.LogInformation($"Rol oluşturuldu: {role.Name} - {role.Description}");
                    }
                }

                if (createdRoles > 0)
                {
                    _logger.LogInformation($"{createdRoles} yeni rol oluşturuldu.");
                }
                else
                {
                    _logger.LogInformation("Tüm roller zaten mevcut.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Roller oluşturulurken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Admin kullanıcısını oluştur
        /// </summary>
        public async Task CreateAdminUserAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                // Varsayılan kullanıcıları oluştur
                var defaultUsers = new[]
                {
                    new { Email = "admin@newspaper.com", Password = "Super123!", FirstName = "Super", LastName = "Admin", Role = "SuperAdmin" },
                    new { Email = "manager@newspaper.com", Password = "Manager123!", FirstName = "Site", LastName = "Manager", Role = "Admin" },
                    new { Email = "editor@newspaper.com", Password = "Editor123!", FirstName = "Content", LastName = "Editor", Role = "Editor" },
                    new { Email = "author@newspaper.com", Password = "Author123!", FirstName = "Content", LastName = "Author", Role = "Author" },
                    new { Email = "moderator@newspaper.com", Password = "Moderator123!", FirstName = "Community", LastName = "Moderator", Role = "Moderator" }
                };

                foreach (var userInfo in defaultUsers)
                {
                    var existingUser = await userManager.FindByEmailAsync(userInfo.Email);
                    if (existingUser == null)
                    {
                        var newUser = new User
                        {
                            UserName = userInfo.Email,
                            Email = userInfo.Email,
                            FirstName = userInfo.FirstName,
                            LastName = userInfo.LastName,
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                            TwoFactorEnabled = false,
                            LockoutEnabled = false,
                            AccessFailedCount = 0,
                            IsActive = true
                        };

                        var result = await userManager.CreateAsync(newUser, userInfo.Password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newUser, userInfo.Role);
                            _logger.LogInformation("Kullanıcı oluşturuldu: {Email} - Rol: {Role}", userInfo.Email, userInfo.Role);
                        }
                        else
                        {
                            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                            _logger.LogError("Kullanıcı oluşturulamadı: {Email} - Hatalar: {Errors}", userInfo.Email, errors);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Kullanıcı zaten mevcut: {Email}", userInfo.Email);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin kullanıcıları oluşturulurken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Kategorileri oluştur
        /// </summary>
        public async Task CreateCategoriesAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<NewspaperDbContext>();

                if (!context.Categories.Any())
                {
                    var categories = new List<Category>
                    {
                        new Category { Name = "Teknoloji", Description = "Teknoloji haberleri", Slug = "teknoloji" },
                        new Category { Name = "Spor", Description = "Spor haberleri", Slug = "spor" },
                        new Category { Name = "Ekonomi", Description = "Ekonomi haberleri", Slug = "ekonomi" },
                        new Category { Name = "Siyaset", Description = "Siyaset haberleri", Slug = "siyaset" },
                        new Category { Name = "Sağlık", Description = "Sağlık haberleri", Slug = "saglik" },
                        new Category { Name = "Eğitim", Description = "Eğitim haberleri", Slug = "egitim" },
                        new Category { Name = "Kültür-Sanat", Description = "Kültür ve sanat haberleri", Slug = "kultur-sanat" },
                        new Category { Name = "Bilim", Description = "Bilim haberleri", Slug = "bilim" }
                    };

                    context.Categories.AddRange(categories);
                    await context.SaveChangesAsync();
                    _logger.LogInformation("{Count} kategori oluşturuldu.", categories.Count);
                }
                else
                {
                    _logger.LogInformation("Kategoriler zaten mevcut.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriler oluşturulurken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Tag'leri oluştur
        /// </summary>
        public async Task CreateTagsAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<NewspaperDbContext>();

                if (!context.Tags.Any())
                {
                    var tags = new List<Tag>
                    {
                        new Tag { Name = "Yapay Zeka", Slug = "yapay-zeka" },
                        new Tag { Name = "Blockchain", Slug = "blockchain" },
                        new Tag { Name = "Futbol", Slug = "futbol" },
                        new Tag { Name = "Basketbol", Slug = "basketbol" },
                        new Tag { Name = "Borsa", Slug = "borsa" },
                        new Tag { Name = "Kripto Para", Slug = "kripto-para" },
                        new Tag { Name = "Sağlık", Slug = "saglik" },
                        new Tag { Name = "Teknoloji", Slug = "teknoloji" },
                        new Tag { Name = "Çevre", Slug = "cevre" },
                        new Tag { Name = "Eğitim", Slug = "egitim" }
                    };

                    context.Tags.AddRange(tags);
                    await context.SaveChangesAsync();
                    _logger.LogInformation("{Count} tag oluşturuldu.", tags.Count);
                }
                else
                {
                    _logger.LogInformation("Tag'ler zaten mevcut.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tag'ler oluşturulurken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Makaleleri oluştur
        /// </summary>
        public async Task CreateArticlesAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<NewspaperDbContext>();

                if (!context.Articles.Any())
                {
                    var categories = context.Categories.ToList();
                    var tags = context.Tags.ToList();
                    var users = context.Users.ToList();

                    if (!categories.Any() || !users.Any())
                    {
                        _logger.LogWarning("Makale oluşturmak için kategori veya kullanıcı bulunamadı.");
                        return;
                    }

                    var articles = new List<Article>();

                    for (int i = 1; i <= 20; i++)
                    {
                        var randomCategory = categories[Random.Shared.Next(categories.Count)];
                        var randomUser = users[Random.Shared.Next(users.Count)];

                        var article = new Article
                        {
                            Title = $"Örnek Makale {i}",
                            Content = $"Bu örnek makale {i} içeriğidir. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                            Summary = $"Örnek makale {i} özeti",
                            Slug = $"ornek-makale-{i}",
                            AuthorId = randomUser.Id,
                            CategoryId = randomCategory.Id,
                            IsPublish = true,
                            ViewCount = Random.Shared.Next(0, 1000)
                        };

                        articles.Add(article);
                    }

                    context.Articles.AddRange(articles);
                    await context.SaveChangesAsync();

                    // ArticleTag ilişkilerini oluştur
                    var articleTags = new List<ArticleTag>();
                    var savedArticles = context.Articles.ToList();

                    foreach (var article in savedArticles)
                    {
                        var randomTags = tags.OrderBy(x => Random.Shared.Next()).Take(Random.Shared.Next(2, 5)).ToList();
                        foreach (var tag in randomTags)
                        {
                            articleTags.Add(new ArticleTag
                            {
                                ArticleId = article.Id,
                                TagId = tag.Id
                            });
                        }
                    }

                    context.ArticleTags.AddRange(articleTags);
                    await context.SaveChangesAsync();

                    _logger.LogInformation("{ArticleCount} makale ve {TagCount} makale-tag ilişkisi oluşturuldu.", articles.Count, articleTags.Count);
                }
                else
                {
                    _logger.LogInformation("Makaleler zaten mevcut.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makaleler oluşturulurken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Yorumları oluştur
        /// </summary>
        public async Task CreateCommentsAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<NewspaperDbContext>();

                if (!context.Comments.Any())
                {
                    var articles = context.Articles.ToList();
                    var users = context.Users.ToList();

                    if (!articles.Any() || !users.Any())
                    {
                        _logger.LogWarning("Yorum oluşturmak için makale veya kullanıcı bulunamadı.");
                        return;
                    }

                    var comments = new List<Comment>();

                    for (int i = 1; i <= 50; i++)
                    {
                        var randomArticle = articles[Random.Shared.Next(articles.Count)];
                        var randomUser = users[Random.Shared.Next(users.Count)];

                        var comment = new Comment
                        {
                            Content = $"Bu örnek yorum {i} içeriğidir. Çok güzel bir makale olmuş!",
                            ArticleId = randomArticle.Id,
                            UserId = randomUser.Id,
                            IsPublish = true,
                            IpAddress = "127.0.0.1",
                            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"
                        };

                        comments.Add(comment);
                    }

                    context.Comments.AddRange(comments);
                    await context.SaveChangesAsync();

                    _logger.LogInformation("{Count} yorum oluşturuldu.", comments.Count);
                }
                else
                {
                    _logger.LogInformation("Yorumlar zaten mevcut.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yorumlar oluşturulurken hata oluştu.");
                throw;
            }
        }

        /// <summary>
        /// Seed data durumunu kontrol et
        /// </summary>
        public async Task<SeedDataStatus> GetSeedDataStatusAsync()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<NewspaperDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var status = new SeedDataStatus
                {
                    HasRoles = await roleManager.Roles.AnyAsync(),
                    HasAdminUser = await userManager.FindByEmailAsync("admin@gmail.com") != null,
                    HasCategories = context.Categories.Any(),
                    HasTags = context.Tags.Any(),
                    HasArticles = context.Articles.Any(),
                    HasComments = context.Comments.Any(),
                    RoleCount = await roleManager.Roles.CountAsync(),
                    CategoryCount = context.Categories.Count(),
                    TagCount = context.Tags.Count(),
                    ArticleCount = context.Articles.Count(),
                    CommentCount = context.Comments.Count()
                };

                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seed data durumu kontrol edilirken hata oluştu.");
                throw;
            }
        }
    }
}
