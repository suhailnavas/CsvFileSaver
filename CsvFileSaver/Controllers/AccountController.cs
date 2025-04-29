using CsvFileSaver.Models;
using CsvFileSaver.Models.Dto;
using CsvFileSaver.Service.IService;
using CsvFileSaver.ViewModel;
using CsvFileSaver.ViewModel.UsersApp.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CsvFileSaver.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CsvFileSaver.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthService _authService;
        public AccountController(ILogger<AccountController> logger,IAuthService authService)
        {
            _authService = authService;
            _logger = logger;
        }
        public async Task<IActionResult> Login()
        {
            
            return View();
        }

        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Roles = new List<SelectListItem>
                {
                    new SelectListItem { Value = "User", Text = "User" },
                    new SelectListItem { Value = "Admin", Text = "Administrator" },                   
                }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ButtonRegister(RegisterViewModel obj)
        {
            try {
                RegisterationRequestDTO Registerobj = new RegisterationRequestDTO
                {
                    Name = obj.Name,
                    Email = obj.Email,
                    Password = obj.Password,
                    Role =obj.SelectedRole                   
                };

                APIResponse result = await _authService.RegisterAsync<APIResponse>(Registerobj);
                if (result != null && result.IsSuccess)
                {
                    return RedirectToAction("Login");
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while submitting the registration. The issue has been logged for further investigation.");
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
                    Password = obj.Password,
                    Role = string.Empty
                };

                APIResponse result = await _authService.LoginAsync<APIResponse>(requestobj);

                if (result != null && result.Result != null && result.IsSuccess)
                {
                    LoginResponceDto model = JsonConvert.DeserializeObject<LoginResponceDto>(Convert.ToString(result.Result));

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(model?.AccessToken);

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "name")?.Value ?? ""));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Session.SetString(Constants.SessionToken, model.AccessToken);
                    HttpContext.Session.SetString(Constants.UserName, model.Name);
                    HttpContext.Session.SetString(Constants.UserId, model.Id);
                    HttpContext.Session.SetString(Constants.UserRole, model.Role);

                    _authService.SetToken(model.AccessToken);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View("Login", obj);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetType().Name, "An error occurred while submitting the Login Request. The issue has been logged for further investigation.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return View("Login", obj);
            }
        }

    }
}
