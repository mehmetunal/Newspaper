using FluentValidation;
using Newspaper.Dto.Mssql;

namespace Newspaper.Api.Validators
{
    /// <summary>
    /// CreateTagDto Validator
    /// </summary>
    public class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
    {
        public CreateTagDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tag adı boş olamaz")
                .MaximumLength(50).WithMessage("Tag adı en fazla 50 karakter olabilir");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug boş olamaz")
                .MaximumLength(50).WithMessage("Slug en fazla 50 karakter olabilir")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }

    /// <summary>
    /// UpdateTagDto Validator
    /// </summary>
    public class UpdateTagDtoValidator : AbstractValidator<UpdateTagDto>
    {
        public UpdateTagDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Tag ID boş olamaz");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tag adı boş olamaz")
                .MaximumLength(50).WithMessage("Tag adı en fazla 50 karakter olabilir");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug boş olamaz")
                .MaximumLength(50).WithMessage("Slug en fazla 50 karakter olabilir")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
} 