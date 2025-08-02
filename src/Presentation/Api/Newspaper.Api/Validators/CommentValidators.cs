using FluentValidation;
using Newspaper.Dto.Mssql;

namespace Newspaper.Api.Validators
{
    /// <summary>
    /// CreateCommentDto Validator
    /// </summary>
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz")
                .MinimumLength(10).WithMessage("Yorum içeriği en az 10 karakter olmalıdır")
                .MaximumLength(1000).WithMessage("Yorum içeriği en fazla 1000 karakter olabilir");

            RuleFor(x => x.ArticleId)
                .NotEmpty().WithMessage("Makale ID boş olamaz");

            RuleFor(x => x.ParentCommentId)
                .NotEqual(Guid.Empty).WithMessage("Geçersiz üst yorum ID'si")
                .When(x => x.ParentCommentId.HasValue);

            RuleFor(x => x.IpAddress)
                .MaximumLength(45).WithMessage("IP adresi en fazla 45 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.IpAddress));

            RuleFor(x => x.UserAgent)
                .MaximumLength(500).WithMessage("User Agent en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.UserAgent));
        }
    }

    /// <summary>
    /// UpdateCommentDto Validator
    /// </summary>
    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Yorum ID boş olamaz");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz")
                .MinimumLength(10).WithMessage("Yorum içeriği en az 10 karakter olmalıdır")
                .MaximumLength(1000).WithMessage("Yorum içeriği en fazla 1000 karakter olabilir");
        }
    }
} 