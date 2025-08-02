using FluentMigrator;
using System;

namespace Newspaper.Mssql.Migrations
{
    /// <summary>
    /// Ana tabloları oluşturan migration
    /// </summary>
    [Migration(002)]
    public class CreateMainTables : Migration
    {
        public override void Up()
        {
            // Categories tablosu
            Create.Table("Categories")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Description").AsString(500).Nullable()
                .WithColumn("Slug").AsString(100).NotNullable()
                .WithColumn("Icon").AsString(50).Nullable()
                .WithColumn("Color").AsString(7).Nullable()
                .WithColumn("ParentCategoryId").AsGuid().Nullable()
                .WithColumn("Order").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublish").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedDate").AsDateTime().Nullable();

            // Tags tablosu
            Create.Table("Tags")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Slug").AsString(100).NotNullable()
                .WithColumn("Description").AsString(500).Nullable()
                .WithColumn("UsageCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublish").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedDate").AsDateTime().Nullable();

            // Articles tablosu
            Create.Table("Articles")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Title").AsString(200).NotNullable()
                .WithColumn("Content").AsString().NotNullable()
                .WithColumn("Summary").AsString(500).Nullable()
                .WithColumn("Slug").AsString(200).NotNullable()
                .WithColumn("CoverImageUrl").AsString(500).Nullable()
                .WithColumn("MetaKeywords").AsString(500).Nullable()
                .WithColumn("MetaDescription").AsString(500).Nullable()
                .WithColumn("AuthorId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable()
                .WithColumn("PublishedAt").AsDateTime().Nullable()
                .WithColumn("IsFeatured").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsOnHomePage").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("ReadingTime").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("ViewCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("LikeCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("CommentCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("ShareCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublish").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedDate").AsDateTime().Nullable();

            // ArticleTags tablosu
            Create.Table("ArticleTags")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("ArticleId").AsGuid().NotNullable()
                .WithColumn("TagId").AsGuid().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedDate").AsDateTime().Nullable();

            // Comments tablosu
            Create.Table("Comments")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Content").AsString().NotNullable()
                .WithColumn("ArticleId").AsGuid().NotNullable()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("ParentCommentId").AsGuid().Nullable()
                .WithColumn("LikeCount").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("IpAddress").AsString(45).Nullable()
                .WithColumn("UserAgent").AsString(500).Nullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublish").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedDate").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
                .WithColumn("ModifiedDate").AsDateTime().Nullable();

            // Indexes
            Create.Index("IX_Categories_ParentCategoryId")
                .OnTable("Categories")
                .OnColumn("ParentCategoryId");

            Create.Index("IX_Categories_Slug")
                .OnTable("Categories")
                .OnColumn("Slug")
                .Unique();

            Create.Index("IX_Tags_Slug")
                .OnTable("Tags")
                .OnColumn("Slug")
                .Unique();

            Create.Index("IX_Articles_AuthorId")
                .OnTable("Articles")
                .OnColumn("AuthorId");

            Create.Index("IX_Articles_CategoryId")
                .OnTable("Articles")
                .OnColumn("CategoryId");

            Create.Index("IX_Articles_Slug")
                .OnTable("Articles")
                .OnColumn("Slug")
                .Unique();

            Create.Index("IX_Articles_PublishedAt")
                .OnTable("Articles")
                .OnColumn("PublishedAt");

            Create.Index("IX_ArticleTags_ArticleId")
                .OnTable("ArticleTags")
                .OnColumn("ArticleId");

            Create.Index("IX_ArticleTags_TagId")
                .OnTable("ArticleTags")
                .OnColumn("TagId");

            Create.Index("IX_Comments_ArticleId")
                .OnTable("Comments")
                .OnColumn("ArticleId");

            Create.Index("IX_Comments_UserId")
                .OnTable("Comments")
                .OnColumn("UserId");

            Create.Index("IX_Comments_ParentCommentId")
                .OnTable("Comments")
                .OnColumn("ParentCommentId");

            // Foreign Keys
            Create.ForeignKey("FK_Categories_Categories_ParentCategoryId")
                .FromTable("Categories").ForeignColumn("ParentCategoryId")
                .ToTable("Categories").PrimaryColumn("Id");

            Create.ForeignKey("FK_Articles_AspNetUsers_AuthorId")
                .FromTable("Articles").ForeignColumn("AuthorId")
                .ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.ForeignKey("FK_Articles_Categories_CategoryId")
                .FromTable("Articles").ForeignColumn("CategoryId")
                .ToTable("Categories").PrimaryColumn("Id");

            Create.ForeignKey("FK_ArticleTags_Articles_ArticleId")
                .FromTable("ArticleTags").ForeignColumn("ArticleId")
                .ToTable("Articles").PrimaryColumn("Id");

            Create.ForeignKey("FK_ArticleTags_Tags_TagId")
                .FromTable("ArticleTags").ForeignColumn("TagId")
                .ToTable("Tags").PrimaryColumn("Id");

            Create.ForeignKey("FK_Comments_Articles_ArticleId")
                .FromTable("Comments").ForeignColumn("ArticleId")
                .ToTable("Articles").PrimaryColumn("Id");

            Create.ForeignKey("FK_Comments_AspNetUsers_UserId")
                .FromTable("Comments").ForeignColumn("UserId")
                .ToTable("AspNetUsers").PrimaryColumn("Id");

            Create.ForeignKey("FK_Comments_Comments_ParentCommentId")
                .FromTable("Comments").ForeignColumn("ParentCommentId")
                .ToTable("Comments").PrimaryColumn("Id");
        }

        public override void Down()
        {
            // Foreign Keys'leri sil
            Delete.ForeignKey("FK_Comments_Comments_ParentCommentId").OnTable("Comments");
            Delete.ForeignKey("FK_Comments_AspNetUsers_UserId").OnTable("Comments");
            Delete.ForeignKey("FK_Comments_Articles_ArticleId").OnTable("Comments");
            Delete.ForeignKey("FK_ArticleTags_Tags_TagId").OnTable("ArticleTags");
            Delete.ForeignKey("FK_ArticleTags_Articles_ArticleId").OnTable("ArticleTags");
            Delete.ForeignKey("FK_Articles_Categories_CategoryId").OnTable("Articles");
            Delete.ForeignKey("FK_Articles_AspNetUsers_AuthorId").OnTable("Articles");
            Delete.ForeignKey("FK_Categories_Categories_ParentCategoryId").OnTable("Categories");

            // Index'leri sil
            Delete.Index("IX_Comments_ParentCommentId").OnTable("Comments");
            Delete.Index("IX_Comments_UserId").OnTable("Comments");
            Delete.Index("IX_Comments_ArticleId").OnTable("Comments");
            Delete.Index("IX_ArticleTags_TagId").OnTable("ArticleTags");
            Delete.Index("IX_ArticleTags_ArticleId").OnTable("ArticleTags");
            Delete.Index("IX_Articles_PublishedAt").OnTable("Articles");
            Delete.Index("IX_Articles_Slug").OnTable("Articles");
            Delete.Index("IX_Articles_CategoryId").OnTable("Articles");
            Delete.Index("IX_Articles_AuthorId").OnTable("Articles");
            Delete.Index("IX_Tags_Slug").OnTable("Tags");
            Delete.Index("IX_Categories_Slug").OnTable("Categories");
            Delete.Index("IX_Categories_ParentCategoryId").OnTable("Categories");

            // Tabloları sil
            Delete.Table("Comments");
            Delete.Table("ArticleTags");
            Delete.Table("Articles");
            Delete.Table("Tags");
            Delete.Table("Categories");
        }
    }
} 