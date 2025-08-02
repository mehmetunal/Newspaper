using FluentMigrator;
using FluentMigrator.Postgres;
using System;
using System.Data;

namespace Newspaper.Mssql.Migrations
{
    /// <summary>
    /// Identity tablolarını oluşturan migration
    /// </summary>
    [Migration(001)]
    public class CreateIdentityTables : Migration
    {
        public override void Up()
        {
            // AspNetRoles tablosu
            Create.Table("AspNetRoles")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(256).Nullable()
                .WithColumn("NormalizedName").AsString(256).Nullable()
                .WithColumn("ConcurrencyStamp").AsString().Nullable();

            // AspNetUsers tablosu
            Create.Table("AspNetUsers")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("UserName").AsString(256).Nullable()
                .WithColumn("NormalizedUserName").AsString(256).Nullable()
                .WithColumn("Email").AsString(256).Nullable()
                .WithColumn("NormalizedEmail").AsString(256).Nullable()
                .WithColumn("EmailConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("PasswordHash").AsString().Nullable()
                .WithColumn("SecurityStamp").AsString().Nullable()
                .WithColumn("ConcurrencyStamp").AsString().Nullable()
                .WithColumn("PhoneNumber").AsString().Nullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("LockoutEnd").AsDateTimeOffset().Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("AccessFailedCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("FirstName").AsString(50).NotNullable()
                .WithColumn("LastName").AsString(50).NotNullable()
                .WithColumn("ProfileImageUrl").AsString(500).Nullable()
                .WithColumn("Biography").AsString(1000).Nullable()
                .WithColumn("BirthDate").AsDateTime().Nullable()
                .WithColumn("Gender").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("LastLoginDate").AsDateTime().Nullable();

            // AspNetUserRoles tablosu
            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("RoleId").AsGuid().NotNullable();

            // AspNetUserRoles primary key
            Create.PrimaryKey("PK_AspNetUserRoles")
                .OnTable("AspNetUserRoles")
                .Columns("UserId", "RoleId");

            // AspNetUserClaims tablosu
            Create.Table("AspNetUserClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable();

            // AspNetUserLogins tablosu
            Create.Table("AspNetUserLogins")
                .WithColumn("LoginProvider").AsString(450).NotNullable()
                .WithColumn("ProviderKey").AsString(450).NotNullable()
                .WithColumn("ProviderDisplayName").AsString().Nullable()
                .WithColumn("UserId").AsGuid().NotNullable();

            // AspNetUserLogins primary key
            Create.PrimaryKey("PK_AspNetUserLogins")
                .OnTable("AspNetUserLogins")
                .Columns("LoginProvider", "ProviderKey");

            // AspNetUserTokens tablosu
            Create.Table("AspNetUserTokens")
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("LoginProvider").AsString(450).NotNullable()
                .WithColumn("Name").AsString(450).NotNullable()
                .WithColumn("Value").AsString().Nullable();

            // AspNetUserTokens primary key
            Create.PrimaryKey("PK_AspNetUserTokens")
                .OnTable("AspNetUserTokens")
                .Columns("UserId", "LoginProvider", "Name");

            // AspNetRoleClaims tablosu
            Create.Table("AspNetRoleClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("RoleId").AsGuid().NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable();

            // Indexes
            Create.Index("IX_AspNetRoles_NormalizedName")
                .OnTable("AspNetRoles")
                .OnColumn("NormalizedName")
                .Unique();

            Create.Index("IX_AspNetUsers_NormalizedUserName")
                .OnTable("AspNetUsers")
                .OnColumn("NormalizedUserName")
                .Unique();

            Create.Index("IX_AspNetUsers_NormalizedEmail")
                .OnTable("AspNetUsers")
                .OnColumn("NormalizedEmail")
                .Unique();

            Create.Index("IX_AspNetUserClaims_UserId")
                .OnTable("AspNetUserClaims")
                .OnColumn("UserId");

            Create.Index("IX_AspNetUserLogins_UserId")
                .OnTable("AspNetUserLogins")
                .OnColumn("UserId");

            Create.Index("IX_AspNetUserRoles_RoleId")
                .OnTable("AspNetUserRoles")
                .OnColumn("RoleId");

            Create.Index("IX_AspNetRoleClaims_RoleId")
                .OnTable("AspNetRoleClaims")
                .OnColumn("RoleId");

            // Foreign Keys
            Create.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId")
                .FromTable("AspNetUserClaims").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId")
                .FromTable("AspNetUserLogins").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId")
                .FromTable("AspNetUserRoles").ForeignColumn("RoleId")
                .ToTable("AspNetRoles").PrimaryColumn("Id");

            Create.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId")
                .FromTable("AspNetUserRoles").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId")
                .FromTable("AspNetUserTokens").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId")
                .FromTable("AspNetRoleClaims").ForeignColumn("RoleId")
                .ToTable("AspNetRoles").PrimaryColumn("Id");
        }

        public override void Down()
        {
            // Foreign Keys'leri sil
            Delete.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId").OnTable("AspNetRoleClaims");
            Delete.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId").OnTable("AspNetUserTokens");
            Delete.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId").OnTable("AspNetUserRoles");
            Delete.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId").OnTable("AspNetUserRoles");
            Delete.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId").OnTable("AspNetUserLogins");
            Delete.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId").OnTable("AspNetUserClaims");

            // Index'leri sil
            Delete.Index("IX_AspNetRoleClaims_RoleId").OnTable("AspNetRoleClaims");
            Delete.Index("IX_AspNetUserRoles_RoleId").OnTable("AspNetUserRoles");
            Delete.Index("IX_AspNetUserLogins_UserId").OnTable("AspNetUserLogins");
            Delete.Index("IX_AspNetUserClaims_UserId").OnTable("AspNetUserClaims");
            Delete.Index("IX_AspNetUsers_NormalizedEmail").OnTable("AspNetUsers");
            Delete.Index("IX_AspNetUsers_NormalizedUserName").OnTable("AspNetUsers");
            Delete.Index("IX_AspNetRoles_NormalizedName").OnTable("AspNetRoles");

            // Tabloları sil
            Delete.Table("AspNetRoleClaims");
            Delete.Table("AspNetUserTokens");
            Delete.Table("AspNetUserLogins");
            Delete.Table("AspNetUserClaims");
            Delete.Table("AspNetUserRoles");
            Delete.Table("AspNetUsers");
            Delete.Table("AspNetRoles");
        }
    }
} 