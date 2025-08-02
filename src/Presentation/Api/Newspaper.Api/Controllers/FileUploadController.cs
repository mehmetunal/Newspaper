using Maggsoft.Core.Base;
using Newspaper.Dto.Mssql;
using Microsoft.AspNetCore.Mvc;
using Maggsoft.Core.IO;

namespace Newspaper.Api.Controllers
{
    /// <summary>
    /// Dosya yükleme işlemleri için controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController : BaseController
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IFilesManager _filesManager;
        private readonly IMaggsoftFileProvider _fileProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileUploadController(
            ILogger<FileUploadController> logger,
            IWebHostEnvironment environment,
            IFilesManager filesManager,
            IMaggsoftFileProvider fileProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _environment = environment;
            _filesManager = filesManager;
            _fileProvider = fileProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Genel dosya yükleme
        /// </summary>
        [HttpPost("upload")]
        public async Task<ActionResult<Result<FileUploadResultDto>>> UploadFile([FromForm] FileUploadDto dto)
        {
            try
            {
                if (dto.File == null || dto.File.Length == 0)
                {
                    return Error<FileUploadResultDto>("Dosya seçilmedi.");
                }

                var fileGuidId = Guid.NewGuid().ToString();
                var virtualPath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/content/files/";
                var fileExtension = _fileProvider.GetFileExtension(dto.File.FileName);
                var filePath = $"{virtualPath}{fileGuidId}{fileExtension}";

                var folderPath = Path.Combine(_environment.WebRootPath, "content", "files");
                _filesManager.FolderCreate(folderPath);
                _filesManager.FilesCreate(folderPath, dto.File, fileGuidId);

                var result = new FileUploadResultDto
                {
                    Success = true,
                    FileName = $"{fileGuidId}{fileExtension}",
                    FileUrl = filePath,
                    FileSize = dto.File.Length,
                    ContentType = dto.File.ContentType
                };

                _logger.LogInformation("Dosya başarıyla yüklendi: {FileName}", result.FileName);
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dosya yükleme sırasında hata oluştu");
                return Error<FileUploadResultDto>("Dosya yükleme sırasında hata oluştu.");
            }
        }

        /// <summary>
        /// Resim yükleme
        /// </summary>
        [HttpPost("upload-image")]
        public async Task<ActionResult<Result<FileUploadResultDto>>> UploadImage([FromForm] ImageUploadDto dto)
        {
            try
            {
                if (dto.Image == null || dto.Image.Length == 0)
                {
                    return Error<FileUploadResultDto>("Resim seçilmedi.");
                }

                var fileGuidId = Guid.NewGuid().ToString();
                var virtualPath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/content/images/";
                var fileExtension = _fileProvider.GetFileExtension(dto.Image.FileName);
                var filePath = $"{virtualPath}{fileGuidId}{fileExtension}";

                var folderPath = Path.Combine(_environment.WebRootPath, "content", "images");
                _filesManager.FolderCreate(folderPath);
                _filesManager.FilesCreate(folderPath, dto.Image, fileGuidId);

                var result = new FileUploadResultDto
                {
                    Success = true,
                    FileName = $"{fileGuidId}{fileExtension}",
                    FileUrl = filePath,
                    FileSize = dto.Image.Length,
                    ContentType = dto.Image.ContentType
                };

                _logger.LogInformation("Resim başarıyla yüklendi: {FileName}", result.FileName);
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resim yükleme sırasında hata oluştu");
                return Error<FileUploadResultDto>("Resim yükleme sırasında hata oluştu.");
            }
        }

        /// <summary>
        /// Profil resmi yükleme
        /// </summary>
        [HttpPost("upload-profile-image")]
        public async Task<ActionResult<Result<FileUploadResultDto>>> UploadProfileImage([FromForm] ProfileImageUploadDto dto)
        {
            try
            {
                if (dto.ProfileImage == null || dto.ProfileImage.Length == 0)
                {
                    return Error<FileUploadResultDto>("Profil resmi seçilmedi.");
                }

                var fileGuidId = Guid.NewGuid().ToString();
                var virtualPath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/content/profiles/";
                var fileExtension = _fileProvider.GetFileExtension(dto.ProfileImage.FileName);
                var filePath = $"{virtualPath}{fileGuidId}{fileExtension}";

                var folderPath = Path.Combine(_environment.WebRootPath, "content", "profiles");
                _filesManager.FolderCreate(folderPath);
                _filesManager.FilesCreate(folderPath, dto.ProfileImage, fileGuidId);

                var result = new FileUploadResultDto
                {
                    Success = true,
                    FileName = $"{fileGuidId}{fileExtension}",
                    FileUrl = filePath,
                    FileSize = dto.ProfileImage.Length,
                    ContentType = dto.ProfileImage.ContentType
                };

                _logger.LogInformation("Profil resmi başarıyla yüklendi: {FileName}", result.FileName);
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Profil resmi yükleme sırasında hata oluştu");
                return Error<FileUploadResultDto>("Profil resmi yükleme sırasında hata oluştu.");
            }
        }

        /// <summary>
        /// Makale resmi yükleme
        /// </summary>
        [HttpPost("upload-article-image")]
        public async Task<ActionResult<Result<FileUploadResultDto>>> UploadArticleImage([FromForm] ArticleImageUploadDto dto)
        {
            try
            {
                if (dto.ArticleImage == null || dto.ArticleImage.Length == 0)
                {
                    return Error<FileUploadResultDto>("Makale resmi seçilmedi.");
                }

                var fileGuidId = Guid.NewGuid().ToString();
                var virtualPath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/content/articles/";
                var fileExtension = _fileProvider.GetFileExtension(dto.ArticleImage.FileName);
                var filePath = $"{virtualPath}{fileGuidId}{fileExtension}";

                var folderPath = Path.Combine(_environment.WebRootPath, "content", "articles");
                _filesManager.FolderCreate(folderPath);
                _filesManager.FilesCreate(folderPath, dto.ArticleImage, fileGuidId);

                var result = new FileUploadResultDto
                {
                    Success = true,
                    FileName = $"{fileGuidId}{fileExtension}",
                    FileUrl = filePath,
                    FileSize = dto.ArticleImage.Length,
                    ContentType = dto.ArticleImage.ContentType
                };

                _logger.LogInformation("Makale resmi başarıyla yüklendi: {FileName} - Makale ID: {ArticleId}", result.FileName, dto.ArticleId);
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Makale resmi yükleme sırasında hata oluştu");
                return Error<FileUploadResultDto>("Makale resmi yükleme sırasında hata oluştu.");
            }
        }

        /// <summary>
        /// Dosya silme
        /// </summary>
        [HttpDelete("delete/{fileName}")]
        public ActionResult<Result<object>> DeleteFile(string fileName, [FromQuery] string folder = "files")
        {
            try
            {
                var folderPath = Path.Combine(_environment.WebRootPath, "content", folder);
                var filePath = Path.Combine(folderPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return Error<object>("Dosya bulunamadı.");
                }

                System.IO.File.Delete(filePath);
                _logger.LogInformation("Dosya başarıyla silindi: {FileName}", fileName);
                return Success<object>(new { success = true, message = "Dosya başarıyla silindi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dosya silme sırasında hata oluştu");
                return Error<object>("Dosya silme sırasında hata oluştu.");
            }
        }
    }
}
