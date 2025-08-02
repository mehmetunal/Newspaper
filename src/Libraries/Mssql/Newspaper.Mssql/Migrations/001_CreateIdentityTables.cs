using System.Data;
using FluentMigrator;
using FluentMigrator.SqlServer;

namespace Newspaper.Mssql.Migrations
{
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
                .WithColumn("ConcurrencyStamp").AsString().Nullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("CreatedAt").AsDateTime2().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("UpdatedAt").AsDateTime2().Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false);

            // AspNetUsers tablosu
            Create.Table("AspNetUsers")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("FirstName").AsString(50).NotNullable()
                .WithColumn("LastName").AsString(50).NotNullable()
                .WithColumn("ProfileImageUrl").AsString(500).Nullable()
                .WithColumn("Biography").AsString(1000).Nullable()
                .WithColumn("BirthDate").AsDateTime2().Nullable()
                .WithColumn("Gender").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("LastLoginDate").AsDateTime2().Nullable()
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("UserName").AsString(256).Nullable()
                .WithColumn("NormalizedUserName").AsString(256).Nullable()
                .WithColumn("Email").AsString(256).Nullable()
                .WithColumn("NormalizedEmail").AsString(256).Nullable()
                .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
                .WithColumn("PasswordHash").AsString().Nullable()
                .WithColumn("SecurityStamp").AsString().Nullable()
                .WithColumn("ConcurrencyStamp").AsString().Nullable()
                .WithColumn("PhoneNumber").AsString().Nullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                .WithColumn("LockoutEnd").AsDateTimeOffset().Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount").AsInt32().NotNullable();

            // AspNetRoleClaims tablosu
            Create.Table("AspNetRoleClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("RoleId").AsGuid().NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable();

            // AspNetUserClaims tablosu
            Create.Table("AspNetUserClaims")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("ClaimType").AsString().Nullable()
                .WithColumn("ClaimValue").AsString().Nullable();

            // AspNetUserLogins tablosu
            Create.Table("AspNetUserLogins")
                .WithColumn("LoginProvider").AsString(450).PrimaryKey()
                .WithColumn("ProviderKey").AsString(450).PrimaryKey()
                .WithColumn("ProviderDisplayName").AsString().Nullable()
                .WithColumn("UserId").AsGuid().NotNullable();

            // AspNetUserRoles tablosu
            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsGuid().PrimaryKey()
                .WithColumn("RoleId").AsGuid().PrimaryKey();

            // AspNetUserTokens tablosu
            Create.Table("AspNetUserTokens")
                .WithColumn("UserId").AsGuid().PrimaryKey()
                .WithColumn("LoginProvider").AsString(450).PrimaryKey()
                .WithColumn("Name").AsString(450).PrimaryKey()
                .WithColumn("Value").AsString().Nullable();

            // Index'ler
            Create.Index("RoleNameIndex")
                .OnTable("AspNetRoles")
                .OnColumn("NormalizedName")
                .Unique()
                .WithOptions().Filter("[NormalizedName] IS NOT NULL");

            Create.Index("EmailIndex")
                .OnTable("AspNetUsers")
                .OnColumn("NormalizedEmail");

            Create.Index("UserNameIndex")
                .OnTable("AspNetUsers")
                .OnColumn("NormalizedUserName")
                .Unique()
                .WithOptions().Filter("[NormalizedUserName] IS NOT NULL");

            Create.Index("IX_AspNetRoleClaims_RoleId")
                .OnTable("AspNetRoleClaims")
                .OnColumn("RoleId");

            Create.Index("IX_AspNetUserClaims_UserId")
                .OnTable("AspNetUserClaims")
                .OnColumn("UserId");

            Create.Index("IX_AspNetUserLogins_UserId")
                .OnTable("AspNetUserLogins")
                .OnColumn("UserId");

            Create.Index("IX_AspNetUserRoles_RoleId")
                .OnTable("AspNetUserRoles")
                .OnColumn("RoleId");

            // Foreign Key'ler
            Create.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId")
                .FromTable("AspNetRoleClaims").ForeignColumn("RoleId")
                .ToTable("AspNetRoles").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId")
                .FromTable("AspNetUserClaims").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId")
                .FromTable("AspNetUserLogins").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId")
                .FromTable("AspNetUserRoles").ForeignColumn("RoleId")
                .ToTable("AspNetRoles").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId")
                .FromTable("AspNetUserRoles").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);

            Create.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId")
                .FromTable("AspNetUserTokens").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id")
                .OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            // Foreign Key'leri sil
            Delete.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId").OnTable("AspNetUserTokens");
            Delete.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId").OnTable("AspNetUserRoles");
            Delete.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId").OnTable("AspNetUserRoles");
            Delete.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId").OnTable("AspNetUserLogins");
            Delete.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId").OnTable("AspNetUserClaims");
            Delete.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId").OnTable("AspNetRoleClaims");

            // Index'leri sil
            Delete.Index("IX_AspNetUserRoles_RoleId").OnTable("AspNetUserRoles");
            Delete.Index("IX_AspNetUserLogins_UserId").OnTable("AspNetUserLogins");
            Delete.Index("IX_AspNetUserClaims_UserId").OnTable("AspNetUserClaims");
            Delete.Index("IX_AspNetRoleClaims_RoleId").OnTable("AspNetRoleClaims");
            Delete.Index("UserNameIndex").OnTable("AspNetUsers");
            Delete.Index("EmailIndex").OnTable("AspNetUsers");
            Delete.Index("RoleNameIndex").OnTable("AspNetRoles");

            // Tabloları sil
            Delete.Table("AspNetUserTokens");
            Delete.Table("AspNetUserRoles");
            Delete.Table("AspNetUserLogins");
            Delete.Table("AspNetUserClaims");
            Delete.Table("AspNetRoleClaims");
            Delete.Table("AspNetUsers");
            Delete.Table("AspNetRoles");
        }
    }
}
