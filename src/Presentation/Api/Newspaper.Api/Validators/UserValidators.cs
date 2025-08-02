using FluentValidation;
using Newspaper.Dto.Mssql;
using Newspaper.Dto.Mssql.Common;
using Newspaper.Api.Controllers;

namespace Newspaper.Api.Validators
{
    /// <summary>
    /// CreateUserDto Validator
    /// </summary>
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş olamaz")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir");

            RuleFor(x => x.ProfileImageUrl)
                .MaximumLength(500).WithMessage("Profil resmi URL'si en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.ProfileImageUrl));

            RuleFor(x => x.Biography)
                .MaximumLength(1000).WithMessage("Biyografi en fazla 1000 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Biography));

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roller boş olamaz");
        }
    }

    /// <summary>
    /// UpdateUserDto Validator
    /// </summary>
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Kullanıcı ID boş olamaz");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş olamaz")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
                .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir");

            RuleFor(x => x.ProfileImageUrl)
                .MaximumLength(500).WithMessage("Profil resmi URL'si en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.ProfileImageUrl));

            RuleFor(x => x.Biography)
                .MaximumLength(1000).WithMessage("Biyografi en fazla 1000 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Biography));

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roller boş olamaz");
        }
    }

    /// <summary>
    /// ChangePasswordDto Validator
    /// </summary>
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Kullanıcı ID boş olamaz");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Mevcut şifre boş olamaz");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre alanı boş olamaz")
                .MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır")
                .MaximumLength(100).WithMessage("Yeni şifre en fazla 100 karakter olabilir");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Yeni şifreler eşleşmiyor");
        }
    }

    /// <summary>
    /// LoginDto Validator
    /// </summary>
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz");
        }
    }
}