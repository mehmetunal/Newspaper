using Microsoft.AspNetCore.Identity;

public sealed class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateEmail(string email) => new()
    {
        Code = nameof(DuplicateEmail),
        Description = string.Format("{0} e-postası zaten alınmış.", email)
    };

    public override IdentityError DefaultError() => new()
    {
        Code = nameof(DefaultError),
        Description = "bilinmeyen bir hata meydana geldi."
    };

    public override IdentityError ConcurrencyFailure() => new()
    {
        Code = nameof(ConcurrencyFailure),
        Description = "iyimser eşzamanlılık hatası, nesne değiştirildi."
    };

    public override IdentityError PasswordMismatch() => new()
    {
        Code = nameof(PasswordMismatch),
        Description = "yanlış parola."
    };

    public override IdentityError InvalidToken() => new()
    {
        Code = nameof(InvalidToken),
        Description = "geçersiz jeton."
    };

    public override IdentityError LoginAlreadyAssociated() => new()
    {
        Code = nameof(LoginAlreadyAssociated),
        Description = "bu girişe sahip bir kullanıcı zaten var."
    };

    public override IdentityError InvalidUserName(string userName) => new()
    {
        Code = nameof(InvalidUserName),
        Description = string.Format("{0} kullanıcı adı geçersiz, yalnızca harf veya rakam içerebilir.", userName)
    };

    public override IdentityError InvalidEmail(string email) => new()
    {
        Code = nameof(InvalidEmail),
        Description = string.Format("{0} e-postası geçersiz.", email)
    };

    public override IdentityError DuplicateUserName(string userName) => new()
    {
        Code = nameof(DuplicateUserName),
        Description = string.Format("{0} kullanıcı adı zaten alınmış.", userName)
    };

    public override IdentityError InvalidRoleName(string role) => new()
    {
        Code = nameof(InvalidRoleName),
        Description = string.Format("{0} rol adı geçersiz.", role)
    };

    public override IdentityError DuplicateRoleName(string role) => new()
    {
        Code = nameof(DuplicateRoleName),
        Description = string.Format("{0} rol adı zaten alınmış.", role)
    };

    public override IdentityError UserAlreadyHasPassword() => new()
    {
        Code = nameof(UserAlreadyHasPassword),
        Description = "kullanıcının zaten ayarlanmış bir şifresi var."
    };

    public override IdentityError UserLockoutNotEnabled() => new()
    {
        Code = nameof(UserLockoutNotEnabled),
        Description = "kilitleme bu kullanıcı için etkin değil."
    };

    public override IdentityError UserAlreadyInRole(string role) => new()
    {
        Code = nameof(UserAlreadyInRole),
        Description = string.Format("kullanıcı zaten {0} rolünde.", role)
    };

    public override IdentityError UserNotInRole(string role) => new()
    {
        Code = nameof(UserNotInRole),
        Description = string.Format("kullanıcı {0} rolünde değil.", role)
    };

    public override IdentityError PasswordTooShort(int length) => new()
    {
        Code = nameof(PasswordTooShort),
        Description = string.Format("şifreler en az {0} karakter olmalıdır.", length)
    };

    public override IdentityError PasswordRequiresNonAlphanumeric() => new()
    {
        Code = nameof(PasswordRequiresNonAlphanumeric),
        Description = "şifreler en az bir alfasayısal olmayan karakter içermelidir."
    };

    public override IdentityError PasswordRequiresDigit() => new()
    {
        Code = nameof(PasswordRequiresDigit),
        Description = "şifreler en az bir rakamdan ('0'-'9') oluşmalıdır."
    };

    public override IdentityError PasswordRequiresLower() => new()
    {
        Code = nameof(PasswordRequiresLower),
        Description = "şifreler en az bir küçük harfe ('a'-'z') sahip olmalıdır."
    };

    public override IdentityError PasswordRequiresUpper() => new()
    {
        Code = nameof(PasswordRequiresUpper),
        Description = "şifreler en az bir büyük harfe ('a'-'z') sahip olmalıdır."
    };

}