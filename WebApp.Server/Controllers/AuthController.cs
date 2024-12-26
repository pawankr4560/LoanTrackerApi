using Microsoft.AspNetCore.Mvc;
using WebApp.Model.Auth;
using WebApp.Model.Common;
using WebApp.Service.Auth;

namespace WebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup(SignUpRequestModel model)
        {
            try
            {
                var result = await _authService.SignUpUser(model);
                return Ok(new ApiResponse(true, "User signup successfully", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message, ex));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            try
            {
                var result = await _authService.Login(model);
                return Ok(new ApiResponse(true, "Login Successfull", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message, null));
            }
        }
    }
}
