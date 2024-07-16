using Microsoft.AspNetCore.Mvc;
using PasswordHashedDemo_DotNetAPI.Models;
using PasswordHashedDemo_DotNetAPI.Services;

namespace PasswordHashedDemo_DotNetAPI.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;
        public AuthenticationController(ApplicationDbContext DbContext,IPasswordHashService passwordHashService) { 
            _dbContext = DbContext;
            _passwordHashService = passwordHashService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(UserRegisterDTO user)
        {
            bool isSuccess= await _passwordHashService.HashPassword(user);
            if (isSuccess)
            {
                return Created();
            }

            return BadRequest("user could not be Registered");
        }

        [HttpGet("Login")]
        public async Task<ActionResult<Boolean>> LoginUser(UserLoginDTO userLogin)
        {
            bool isSuccess = await _passwordHashService.ValidatePassWord(userLogin);
            if (isSuccess)
            {
                return Ok(true);
            }

            return Ok(false);
        }

    }
}
