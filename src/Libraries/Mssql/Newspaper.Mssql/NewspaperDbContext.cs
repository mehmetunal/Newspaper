using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newspaper.Data.Mssql;

namespace Newspaper.Mssql
{
    /// <summary>
    /// Ana veritabanı context sınıfı
    /// </summary>
    public class NewspaperDbContext : IdentityDbContext<User, Role, Guid>
    {
        public NewspaperDbContext(DbContextOptions<NewspaperDbContext> options) : base(options)
        {
             ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // DbSet'ler
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User entity konfigürasyonu
            builder.Entity<User>(entity =>
            {
                entity.ToTable("AspNetUsers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);
                entity.Property(e => e.Biography).HasMaxLength(1000);
                entity.Property(e => e.Gender).HasDefaultValue(0);
                entity.Property(e => e.LastLoginDate);
            });

            // Role entity konfigürasyonu
            builder.Entity<Role>(entity =>
            {
                entity.ToTable("AspNetRoles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });

            // Category entity konfigürasyonu
            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Icon).HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(7);
                entity.Property(e => e.Order).HasDefaultValue(0);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsPublish).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Self-referencing relationship (hierarchical categories)
                entity.HasOne(e => e.ParentCategory)
                    .WithMany(e => e.SubCategories)
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Article entity konfigürasyonu
            builder.Entity<Article>(entity =>
            {
                entity.ToTable("Articles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Summary).HasMaxLength(500);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
                entity.Property(e => e.MetaKeywords).HasMaxLength(500);
                entity.Property(e => e.MetaDescription).HasMaxLength(500);
                entity.Property(e => e.ViewCount).HasDefaultValue(0);
                entity.Property(e => e.LikeCount).HasDefaultValue(0);
                entity.Property(e => e.CommentCount).HasDefaultValue(0);
                entity.Property(e => e.ShareCount).HasDefaultValue(0);
                entity.Property(e => e.IsFeatured).HasDefaultValue(false);
                entity.Property(e => e.IsOnHomePage).HasDefaultValue(false);
                entity.Property(e => e.Status).HasDefaultValue(0);
                entity.Property(e => e.ReadingTime).HasDefaultValue(0);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsPublish).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Relationships
                entity.HasOne(e => e.Author)
                    .WithMany(e => e.Articles)
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Articles)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Tag entity konfigürasyonu
            builder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tags");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.UsageCount).HasDefaultValue(0);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsPublish).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Unique constraints
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.Slug).IsUnique();
            });

            // ArticleTag entity konfigürasyonu (Many-to-Many)
            builder.Entity<ArticleTag>(entity =>
            {
                entity.ToTable("ArticleTags");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsPublish).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Relationships
                entity.HasOne(e => e.Article)
                    .WithMany(e => e.ArticleTags)
                    .HasForeignKey(e => e.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tag)
                    .WithMany(e => e.ArticleTags)
                    .HasForeignKey(e => e.TagId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Unique constraint
                entity.HasIndex(e => new { e.ArticleId, e.TagId }).IsUnique();
            });

            // Comment entity konfigürasyonu
            builder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.LikeCount).HasDefaultValue(0);
                entity.Property(e => e.Status).HasDefaultValue(0);
                entity.Property(e => e.IpAddress).HasMaxLength(45);
                entity.Property(e => e.UserAgent).HasMaxLength(500);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsPublish).HasDefaultValue(true);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Relationships
                entity.HasOne(e => e.Article)
                    .WithMany(e => e.Comments)
                    .HasForeignKey(e => e.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Comments)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Self-referencing relationship (replies)
                entity.HasOne(e => e.ParentComment)
                    .WithMany(e => e.Replies)
                    .HasForeignKey(e => e.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 