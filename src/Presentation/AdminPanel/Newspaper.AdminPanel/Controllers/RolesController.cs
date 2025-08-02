using Maggsoft.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newspaper.AdminPanel.Models;
using Newspaper.Dto.Mssql;
using Maggsoft.Framework.HttpClientApi;
using Newspaper.Dto.Mssql.Role;

namespace Newspaper.AdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class RolesController : Controller
    {
        private readonly IMaggsoftHttpClient _httpClient;

        public RolesController(IMaggsoftHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Rol Yönetimi";

            try
            {
                var response = await _httpClient.GetAsync<Result<List<RoleDto>>>("api/role");
                if (response.IsSuccess)
                {
                    return View(response.Data);
                }

                TempData["Error"] = "Roller yüklenirken hata oluştu.";
                return View(new List<RoleDto>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Roller yüklenirken hata oluştu: " + ex.Message;
                return View(new List<RoleDto>());
            }
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Yeni Rol Oluştur";
            return View(new CreateRoleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createRoleDto = new CreateRoleDto
                    {
                        Name = model.Name,
                        Description = model.Description
                    };

                    var response = await _httpClient.PostAsync<Result<RoleDto>>("api/role", createRoleDto);
                    if (response.IsSuccess)
                    {
                        TempData["Success"] = "Rol başarıyla oluşturuldu.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = response.Message ?? "Rol oluşturulurken hata oluştu.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Rol oluşturulurken hata oluştu: " + ex.Message;
                }
            }

            ViewData["Title"] = "Yeni Rol Oluştur";
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["Title"] = "Rol Düzenle";

            try
            {
                var response = await _httpClient.GetAsync<Result<RoleDto>>($"api/role/{id}");
                if (response.IsSuccess)
                {
                    var model = new EditRoleViewModel
                    {
                        Id = response.Data.Id,
                        Name = response.Data.Name,
                        Description = response.Data.Description,
                        IsActive = response.Data.IsActive
                    };
                    return View(model);
                }

                TempData["Error"] = "Rol bulunamadı.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Rol yüklenirken hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateRoleDto = new UpdateRoleDto
                    {
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive
                    };

                    var response = await _httpClient.PutAsync<Result<RoleDto>>($"api/role/{model.Id}", updateRoleDto);
                    if (response.IsSuccess)
                    {
                        TempData["Success"] = "Rol başarıyla güncellendi.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Error"] = response.Message ?? "Rol güncellenirken hata oluştu.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Rol güncellenirken hata oluştu: " + ex.Message;
                }
            }

            ViewData["Title"] = "Rol Düzenle";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync<Result<object>>($"api/role/{id}");
                if (response.IsSuccess)
                {
                    TempData["Success"] = "Rol başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = response.Message ?? "Rol silinirken hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Rol silinirken hata oluştu: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
