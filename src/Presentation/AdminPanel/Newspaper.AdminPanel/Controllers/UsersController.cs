using Maggsoft.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Models;
using Newspaper.Dto.Mssql;
using Maggsoft.Framework.HttpClientApi;
using Newspaper.Dto.Mssql.Common;

namespace Newspaper.AdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Moderator")]
    public class UsersController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;

        public UsersController(IMaggsoftHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Kullanıcı Yönetimi";

            try
            {
                var response = await _httpClient.GetAsync<Result<List<UserListDto>>>("api/user");
                if (response.IsSuccess)
                {
                    return View(response.Data);
                }

                TempData["Error"] = "Kullanıcılar yüklenirken hata oluştu.";
                return View(new List<UserListDto>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kullanıcılar yüklenirken hata oluştu: " + ex.Message;
                return View(new List<UserListDto>());
            }
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Kullanıcı Oluştur";
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createUserDto = new CreateUserDto
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber
                    };

                    var response = await _httpClient.PostAsync<Result<UserDetailDto>>("api/user", createUserDto);
                    if (response.IsSuccess)
                    {
                        TempData["Success"] = "Kullanıcı başarıyla oluşturuldu.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = response.Message ?? "Kullanıcı oluşturulurken hata oluştu.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Kullanıcı oluşturulurken hata oluştu: " + ex.Message;
                }
            }

            ViewData["Title"] = "Yeni Kullanıcı Oluştur";
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["Title"] = "Kullanıcı Detayları";

            try
            {
                var response = await _httpClient.GetAsync<Result<UserDetailDto>>($"api/user/{id}");
                if (response.IsSuccess)
                {
                    return View(response.Data);
                }

                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kullanıcı yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["Title"] = "Kullanıcı Düzenle";

            try
            {
                var response = await _httpClient.GetAsync<Result<UserDetailDto>>($"api/user/{id}");
                if (response.IsSuccess)
                {
                    var model = new EditUserViewModel
                    {
                        Id = response.Data.Id,
                        FirstName = response.Data.FirstName,
                        LastName = response.Data.LastName,
                        Email = response.Data.Email,
                        PhoneNumber = response.Data.PhoneNumber,
                        IsActive = response.Data.IsActive
                    };
                    return View(model);
                }

                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kullanıcı yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateUserDto = new UpdateUserDto
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        IsActive = model.IsActive
                    };

                    var response = await _httpClient.PutAsync<Result<UserDetailDto>>($"api/user/{model.Id}", updateUserDto);
                    if (response.IsSuccess)
                    {
                        TempData["Success"] = "Kullanıcı başarıyla güncellendi.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = response.Message ?? "Kullanıcı güncellenirken hata oluştu.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Kullanıcı güncellenirken hata oluştu: " + ex.Message;
                }
            }

            ViewData["Title"] = "Kullanıcı Düzenle";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync<Result<object>>($"api/user/{id}");
                if (response.IsSuccess)
                {
                    TempData["Success"] = "Kullanıcı başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = response.Message ?? "Kullanıcı silinirken hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kullanıcı silinirken hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["Title"] = "Giriş Yap";
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loginDto = new LoginDto
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    var response = await _httpClient.PostAsync<Result<LoginResponseDto>>("api/user/login", loginDto);
                    if (response.IsSuccess)
                    {
                        // TODO: Implement cookie authentication
                        TempData["Success"] = "Başarıyla giriş yaptınız.";
                        return RedirectToAction("Index", "Dashboard");
                    }

                    TempData["Error"] = response.Message ?? "Giriş başarısız.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Giriş yapılırken hata oluştu: " + ex.Message;
                }
            }

            ViewData["Title"] = "Giriş Yap";
            return View(model);
        }

        public IActionResult Logout()
        {
            // TODO: Implement logout
            return RedirectToAction("Login");
        }
    }
}
