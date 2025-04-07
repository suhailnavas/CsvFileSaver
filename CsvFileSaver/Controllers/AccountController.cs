using CsvFileSaver.Models;
using CsvFileSaver.Models.Dto;
using CsvFileSaver.Service.IService;
using CsvFileSaver.ViewModel;
using CsvFileSaver.ViewModel.UsersApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CsvFileSaver.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ButtonRegister(RegisterViewModel obj)
        {
            try {
                RegisterationRequestDTO Registerobj = new RegisterationRequestDTO
                {
                    Name = obj.Name,
                    Email = obj.Email,
                    Password = obj.Password
                };

                APIResponse result = await _authService.RegisterAsync<APIResponse>(Registerobj);
                if (result != null && result.IsSuccess)
                {
                    return RedirectToAction("Login");
                }
            }
            catch(Exception e)
            {

            }
            
            return View("Register");
        }


        [HttpPost]
        public async Task<IActionResult> LoginButton(LoginViewModel obj)
        {
            try
            {
                LoginRequestDto requestobj = new LoginRequestDto
                {
                    Email = obj.Email,
                    Password = obj.Password
                };

                APIResponse result = await _authService.LoginAsync<APIResponse>(requestobj);
                if (result != null && result.IsSuccess)
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception e)
            {

            }

            return View("Register");
        }
    }
}
