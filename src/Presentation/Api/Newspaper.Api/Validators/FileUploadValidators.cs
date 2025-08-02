using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newspaper.Dto.Mssql;
using System.Drawing;

namespace Newspaper.Api.Validators
{
    /// <summary>
    /// FileUploadDto Validator
    /// </summary>
    public class FileUploadDtoValidator : AbstractValidator<FileUploadDto>
    {
        public FileUploadDtoValidator()
        {
            RuleFor(x => x.File)
                .Must(BeAValidFile).WithMessage("Geçersiz dosya formatı veya dosya bozuk.")
                .Must(BeWithinSizeLimit).WithMessage("Dosya boyutu çok büyük.")
                .Must(HaveValidExtension).WithMessage("Geçersiz dosya uzantısı.");

            RuleFor(x => x.MaxFileSizeInMB)
                .GreaterThan(0).WithMessage("Maksimum dosya boyutu 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(100).WithMessage("Maksimum dosya boyutu 100MB'dan küçük olmalıdır.");
        }

        private bool BeAValidFile(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            try
            {
                // Dosya başlığını kontrol et
                if (string.IsNullOrEmpty(file.FileName)) return false;

                // Dosya içeriğini kontrol et
                using var stream = file.OpenReadStream();
                return stream.CanRead && stream.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        private bool BeWithinSizeLimit(FileUploadDto dto, IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            var maxSizeInBytes = dto.MaxFileSizeInMB * 1024 * 1024;
            return file.Length <= maxSizeInBytes;
        }

        private bool HaveValidExtension(FileUploadDto dto, IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;
            if (dto.AllowedExtensions == null || dto.AllowedExtensions.Length == 0) return true;

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return dto.AllowedExtensions.Contains(fileExtension);
        }
    }

    /// <summary>
    /// ImageUploadDto Validator
    /// </summary>
    public class ImageUploadDtoValidator : AbstractValidator<ImageUploadDto>
    {
        public ImageUploadDtoValidator()
        {
            RuleFor(x => x.Image)
                .Must(BeAValidImage).WithMessage("Geçersiz resim formatı. Lütfen geçerli bir resim dosyası yükleyin.")
                .Must(BeWithinSizeLimit).WithMessage("Resim dosyası çok büyük.")
                .Must(HaveValidImageExtension).WithMessage("Geçersiz resim formatı. Sadece JPG, PNG, GIF ve WebP formatları desteklenir.");

            RuleFor(x => x.MaxFileSizeInMB)
                .GreaterThan(0).WithMessage("Maksimum dosya boyutu 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(20).WithMessage("Maksimum dosya boyutu 20MB'dan küçük olmalıdır.");
        }

        private bool BeAValidImage(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            try
            {
                using var stream = file.OpenReadStream();
                using var img = Image.FromStream(stream);
                return img != null;
            }
            catch
            {
                return false;
            }
        }

        private bool BeWithinSizeLimit(ImageUploadDto dto, IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            var maxSizeInBytes = dto.MaxFileSizeInMB * 1024 * 1024;
            return file.Length <= maxSizeInBytes;
        }

        private bool HaveValidImageExtension(ImageUploadDto dto, IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return dto.AllowedExtensions.Contains(fileExtension);
        }
    }

    /// <summary>
    /// ProfileImageUploadDto Validator
    /// </summary>
    public class ProfileImageUploadDtoValidator : AbstractValidator<ProfileImageUploadDto>
    {
        public ProfileImageUploadDtoValidator()
        {
            RuleFor(x => x.ProfileImage)
                .Must(BeAValidImage).WithMessage("Geçersiz resim formatı. Lütfen geçerli bir resim dosyası yükleyin.")
                .Must(BeWithinSizeLimit).WithMessage("Profil resmi çok büyük. Maksimum 2MB olmalıdır.")
                .Must(HaveValidImageExtension).WithMessage("Geçersiz resim formatı. Sadece JPG, PNG ve WebP formatları desteklenir.");
        }

        private bool BeAValidImage(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            try
            {
                using var stream = file.OpenReadStream();
                using var img = Image.FromStream(stream);
                return img != null;
            }
            catch
            {
                return false;
            }
        }

        private bool BeWithinSizeLimit(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            var maxSizeInBytes = 2 * 1024 * 1024; // 2MB
            return file.Length <= maxSizeInBytes;
        }

        private bool HaveValidImageExtension(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }

    /// <summary>
    /// ArticleImageUploadDto Validator
    /// </summary>
    public class ArticleImageUploadDtoValidator : AbstractValidator<ArticleImageUploadDto>
    {
        public ArticleImageUploadDtoValidator()
        {
            RuleFor(x => x.ArticleId)
                .NotEmpty().WithMessage("Makale ID boş olamaz");

            RuleFor(x => x.ArticleImage)
                .Must(BeAValidImage).WithMessage("Geçersiz resim formatı. Lütfen geçerli bir resim dosyası yükleyin.")
                .Must(BeWithinSizeLimit).WithMessage("Makale resmi çok büyük. Maksimum 5MB olmalıdır.")
                .Must(HaveValidImageExtension).WithMessage("Geçersiz resim formatı. Sadece JPG, PNG, GIF ve WebP formatları desteklenir.");
        }

        private bool BeAValidImage(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            try
            {
                using var stream = file.OpenReadStream();
                using var img = Image.FromStream(stream);
                return img != null;
            }
            catch
            {
                return false;
            }
        }

        private bool BeWithinSizeLimit(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            var maxSizeInBytes = 5 * 1024 * 1024; // 5MB
            return file.Length <= maxSizeInBytes;
        }

        private bool HaveValidImageExtension(IFormFile? file)
        {
            if (file == null || file.Length == 0) return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }
} 