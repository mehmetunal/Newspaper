using FluentValidation;
using Newspaper.Dto.Mssql;

namespace Newspaper.Api.Validators
{
    /// <summary>
    /// CreateCategoryDto Validator
    /// </summary>
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug boş olamaz")
                .MaximumLength(100).WithMessage("Slug en fazla 100 karakter olabilir")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir");

            RuleFor(x => x.ParentCategoryId)
                .NotEqual(Guid.Empty).WithMessage("Geçersiz üst kategori ID'si")
                .When(x => x.ParentCategoryId.HasValue);

            RuleFor(x => x.Icon)
                .MaximumLength(50).WithMessage("İkon en fazla 50 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Icon));

            RuleFor(x => x.Color)
                .MaximumLength(7).WithMessage("Renk kodu en fazla 7 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Color));
        }
    }

    /// <summary>
    /// UpdateCategoryDto Validator
    /// </summary>
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Kategori ID boş olamaz");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug boş olamaz")
                .MaximumLength(100).WithMessage("Slug en fazla 100 karakter olabilir")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir");

            RuleFor(x => x.ParentCategoryId)
                .NotEqual(Guid.Empty).WithMessage("Geçersiz üst kategori ID'si")
                .When(x => x.ParentCategoryId.HasValue);

            RuleFor(x => x.Icon)
                .MaximumLength(50).WithMessage("İkon en fazla 50 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Icon));

            RuleFor(x => x.Color)
                .MaximumLength(7).WithMessage("Renk kodu en fazla 7 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Color));
        }
    }
} 