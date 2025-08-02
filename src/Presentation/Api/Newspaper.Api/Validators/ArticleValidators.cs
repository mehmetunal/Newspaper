using FluentValidation;
using Newspaper.Dto.Mssql;

namespace Newspaper.Api.Validators
{
    /// <summary>
    /// CreateArticleDto Validator
    /// </summary>
    public class CreateArticleDtoValidator : AbstractValidator<CreateArticleDto>
    {
        public CreateArticleDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Makale başlığı boş olamaz")
                .MaximumLength(200).WithMessage("Makale başlığı en fazla 200 karakter olabilir");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Makale içeriği boş olamaz")
                .MinimumLength(50).WithMessage("Makale içeriği en az 50 karakter olmalıdır");

            RuleFor(x => x.Summary)
                .MaximumLength(500).WithMessage("Makale özeti en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Summary));

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug boş olamaz")
                .MaximumLength(200).WithMessage("Slug en fazla 200 karakter olabilir")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir");

            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("Yazar ID boş olamaz");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori ID boş olamaz");

            RuleFor(x => x.TagIds)
                .NotNull().WithMessage("Tag listesi boş olamaz")
                .Must(x => x != null && x.Count > 0).WithMessage("En az bir tag seçilmelidir");

            RuleFor(x => x.IsPublish)
                .NotNull().WithMessage("Yayın durumu belirtilmelidir");

            RuleFor(x => x.CoverImageUrl)
                .MaximumLength(500).WithMessage("Kapak resmi URL'si en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.CoverImageUrl));

            RuleFor(x => x.MetaKeywords)
                .MaximumLength(500).WithMessage("Meta anahtar kelimeler en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.MetaKeywords));

            RuleFor(x => x.MetaDescription)
                .MaximumLength(500).WithMessage("Meta açıklama en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.MetaDescription));
        }
    }

    /// <summary>
    /// UpdateArticleDto Validator
    /// </summary>
    public class UpdateArticleDtoValidator : AbstractValidator<UpdateArticleDto>
    {
        public UpdateArticleDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Makale ID boş olamaz");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Makale başlığı boş olamaz")
                .MaximumLength(200).WithMessage("Makale başlığı en fazla 200 karakter olabilir");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Makale içeriği boş olamaz")
                .MinimumLength(50).WithMessage("Makale içeriği en az 50 karakter olmalıdır");

            RuleFor(x => x.Summary)
                .MaximumLength(500).WithMessage("Makale özeti en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Summary));

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug boş olamaz")
                .MaximumLength(200).WithMessage("Slug en fazla 200 karakter olabilir")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori ID boş olamaz");

            RuleFor(x => x.TagIds)
                .NotNull().WithMessage("Tag listesi boş olamaz")
                .Must(x => x != null && x.Count > 0).WithMessage("En az bir tag seçilmelidir");

            RuleFor(x => x.IsPublish)
                .NotNull().WithMessage("Yayın durumu belirtilmelidir");

            RuleFor(x => x.CoverImageUrl)
                .MaximumLength(500).WithMessage("Kapak resmi URL'si en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.CoverImageUrl));

            RuleFor(x => x.MetaKeywords)
                .MaximumLength(500).WithMessage("Meta anahtar kelimeler en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.MetaKeywords));

            RuleFor(x => x.MetaDescription)
                .MaximumLength(500).WithMessage("Meta açıklama en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.MetaDescription));
        }
    }
} 