using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.RequestResponseModels;
using WebApi.Services;

namespace WebApi.Controllers
{
    public class UserController : BaseController
    {

        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, IUserService userService) : base(configuration)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest model)
        {
            var response = await _userService.Login(model);
            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("test " + DateTime.UtcNow.ToLongTimeString());
        }

        [HttpGet("testAuth")]
        public IActionResult TestAuth()
        {
            return Ok("testAuth " + DateTime.UtcNow.ToLongTimeString());
        }

    }
}