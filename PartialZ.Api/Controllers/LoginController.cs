using Microsoft.AspNetCore.Mvc;
using PartialZ.Api.Dtos;
using PartialZ.Api.Services.Interfaces;

namespace PartialZ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegistrationDto registrationDto)
        {
            var result = await this._loginService.Login(registrationDto.Email, registrationDto.Password);
            return Json(result);
        }
    }
}
