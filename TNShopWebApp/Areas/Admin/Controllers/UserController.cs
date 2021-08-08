using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.System.Users;
using TNShopWebApp.Service;

namespace TNShopWebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        [TempData]
        public string Message { get; set; }
        [TempData]
        public string Type { get; set; }
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword = "", int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _userApiClient.GetUserPaging(request);
            return View(data.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetUserById(id);

            return View(result.ResultObject);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                Message = "Đăng ký thành công";
                Type = "success";
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var itemUser = await _userApiClient.GetUserById(id);
            if (itemUser.IsSuccessed)
            {
                return View(itemUser.ResultObject);
            }
            else
            {
                Message = "User không tồn tại";
                Type = "danger";
                return RedirectToAction("Index", "User");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                Message = "Xóa thành công";
                Type = "success";
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _userApiClient.GetUserById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObject;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            else
            {
                Message = "User không tồn tại";
                Type = "danger";
                return RedirectToAction("Index", "User");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                Message = "Cập nhật thành công";
                Type = "success";
                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }
        }
    }
}
