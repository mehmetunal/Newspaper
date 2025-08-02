using AutoMapper;
using Newspaper.Data.Mssql;
using Newspaper.Dto.Mssql;

namespace Newspaper.Mssql.Services
{
    /// <summary>
    /// AutoMapper profil konfigürasyonu
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserListDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => 0));

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()));

            // Category mappings
            CreateMap<Category, CategoryListDto>();
            CreateMap<Category, CategoryDetailDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // Article mappings
            CreateMap<Article, ArticleListDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? $"{src.Author.FirstName} {src.Author.LastName}" : string.Empty))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

            CreateMap<Article, ArticleDetailDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? $"{src.Author.FirstName} {src.Author.LastName}" : string.Empty))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Select(at => at.Tag.Name).ToList() : new List<string>()));

            CreateMap<CreateArticleDto, Article>()
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.ShareCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IsPublish, opt => opt.MapFrom(src => src.IsPublish));

            CreateMap<UpdateArticleDto, Article>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IsPublish, opt => opt.MapFrom(src => src.IsPublish));

            // Tag mappings
            CreateMap<Tag, TagListDto>();
            CreateMap<Tag, TagDetailDto>()
                .ForMember(dest => dest.ArticleCount, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Count(at => !at.IsDeleted) : 0));

            CreateMap<CreateTagDto, Tag>()
                .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(src => 0));

            CreateMap<UpdateTagDto, Tag>();

            // Comment mappings
            CreateMap<Comment, CommentListDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : string.Empty))
                .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : string.Empty));

            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 0)); // Beklemede

            CreateMap<UpdateCommentDto, Comment>();

            // Reverse mappings for updates
            CreateMap<UserDetailDto, UpdateUserDto>();
            CreateMap<CategoryDetailDto, UpdateCategoryDto>();
            CreateMap<ArticleDetailDto, UpdateArticleDto>();
            CreateMap<TagDetailDto, UpdateTagDto>();
            CreateMap<CommentListDto, UpdateCommentDto>();
        }
    }
} 