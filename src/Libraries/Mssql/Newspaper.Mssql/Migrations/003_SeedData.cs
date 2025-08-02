using FluentMigrator;
using System;

namespace Newspaper.Mssql.Migrations
{
    /// <summary>
    /// Seed data migration'ı
    /// </summary>
    [Migration(003)]
    public class SeedData : Migration
    {
        public override void Up()
        {
            // Roller ekle
            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();
            var editorRoleId = Guid.NewGuid();

            Insert.IntoTable("AspNetRoles").Row(new
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

            Insert.IntoTable("AspNetRoles").Row(new
            {
                Id = userRoleId,
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

            Insert.IntoTable("AspNetRoles").Row(new
            {
                Id = editorRoleId,
                Name = "Editor",
                NormalizedName = "EDITOR",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

            // Admin kullanıcısı ekle
            var adminUserId = Guid.NewGuid();
            Insert.IntoTable("AspNetUsers").Row(new
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAELbXpQJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9QJ3x9Q==", // Super123!
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                FirstName = "Admin",
                LastName = "User",
                ProfileImageUrl = "",
                Biography = "Sistem yöneticisi",
                BirthDate = (DateTime?)null,
                Gender = 0,
                LastLoginDate = (DateTime?)null
            });

            // Admin kullanıcısına Admin rolü ata
            Insert.IntoTable("AspNetUserRoles").Row(new
            {
                UserId = adminUserId,
                RoleId = adminRoleId
            });

            // Ana kategoriler ekle
            var technologyCategoryId = Guid.NewGuid();
            var sportsCategoryId = Guid.NewGuid();
            var politicsCategoryId = Guid.NewGuid();
            var economyCategoryId = Guid.NewGuid();
            var healthCategoryId = Guid.NewGuid();

            Insert.IntoTable("Categories").Row(new
            {
                Id = technologyCategoryId,
                Name = "Teknoloji",
                Description = "Teknoloji haberleri ve güncellemeleri",
                Slug = "teknoloji",
                Icon = "fas fa-microchip",
                Color = "#007bff",
                ParentCategoryId = (Guid?)null,
                Order = 1,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            Insert.IntoTable("Categories").Row(new
            {
                Id = sportsCategoryId,
                Name = "Spor",
                Description = "Spor haberleri ve sonuçları",
                Slug = "spor",
                Icon = "fas fa-futbol",
                Color = "#28a745",
                ParentCategoryId = (Guid?)null,
                Order = 2,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            Insert.IntoTable("Categories").Row(new
            {
                Id = politicsCategoryId,
                Name = "Siyaset",
                Description = "Siyasi haberler ve gelişmeler",
                Slug = "siyaset",
                Icon = "fas fa-landmark",
                Color = "#dc3545",
                ParentCategoryId = (Guid?)null,
                Order = 3,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            Insert.IntoTable("Categories").Row(new
            {
                Id = economyCategoryId,
                Name = "Ekonomi",
                Description = "Ekonomi haberleri ve piyasa analizleri",
                Slug = "ekonomi",
                Icon = "fas fa-chart-line",
                Color = "#ffc107",
                ParentCategoryId = (Guid?)null,
                Order = 4,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            Insert.IntoTable("Categories").Row(new
            {
                Id = healthCategoryId,
                Name = "Sağlık",
                Description = "Sağlık haberleri ve bilimsel gelişmeler",
                Slug = "saglik",
                Icon = "fas fa-heartbeat",
                Color = "#e83e8c",
                ParentCategoryId = (Guid?)null,
                Order = 5,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            // Alt kategoriler ekle
            var softwareCategoryId = Guid.NewGuid();
            var hardwareCategoryId = Guid.NewGuid();

            Insert.IntoTable("Categories").Row(new
            {
                Id = softwareCategoryId,
                Name = "Yazılım",
                Description = "Yazılım geliştirme ve teknoloji haberleri",
                Slug = "yazilim",
                Icon = "fas fa-code",
                Color = "#17a2b8",
                ParentCategoryId = technologyCategoryId,
                Order = 1,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            Insert.IntoTable("Categories").Row(new
            {
                Id = hardwareCategoryId,
                Name = "Donanım",
                Description = "Donanım ve elektronik ürün haberleri",
                Slug = "donanim",
                Icon = "fas fa-desktop",
                Color = "#6f42c1",
                ParentCategoryId = technologyCategoryId,
                Order = 2,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            // Örnek etiketler ekle
            var aiTagId = Guid.NewGuid();
            var blockchainTagId = Guid.NewGuid();
            var footballTagId = Guid.NewGuid();

            Insert.IntoTable("Tags").Row(new
            {
                Id = aiTagId,
                Name = "Yapay Zeka",
                Slug = "yapay-zeka",
                Description = "Yapay zeka ve makine öğrenmesi",
                UsageCount = 0,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            Insert.IntoTable("Tags").Row(new
            {
                Id = blockchainTagId,
                Name = "Blockchain",
                Slug = "blockchain",
                Description = "Blockchain teknolojisi",
                UsageCount = 0,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });

            Insert.IntoTable("Tags").Row(new
            {
                Id = footballTagId,
                Name = "Futbol",
                Slug = "futbol",
                Description = "Futbol haberleri",
                UsageCount = 0,
                IsDeleted = false,
                IsPublish = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = (DateTime?)null
            });
        }

        public override void Down()
        {
            // Seed data'yı sil
            Delete.FromTable("Tags").AllRows();
            Delete.FromTable("Categories").AllRows();
            Delete.FromTable("AspNetUserRoles").AllRows();
            Delete.FromTable("AspNetUsers").AllRows();
            Delete.FromTable("AspNetRoles").AllRows();
        }
    }
} 