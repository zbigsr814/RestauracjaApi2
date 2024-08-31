using Microsoft.AspNetCore.Mvc;
using RestauracjaApi2.Interfaces;
using RestauracjaApi2.Models;

namespace RestauracjaApi2.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var token = accountService.GenerateJwt(dto);
            return Ok(token);
        }
    }
}
