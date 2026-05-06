using AbySalto.Mid.Application.User;
using AbySalto.Mid.WebApi.Mappers;
using AbySalto.Mid.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Auth = AbySalto.Mid.Application.Authorization;

namespace AbySalto.Mid.WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Auth.IAuthorizationService _authService;

        public UserController(IUserService userService, Auth.IAuthorizationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest registrationRequest)
        {
            var applicationUser = UserMapper.ToDomain(registrationRequest);
            var result = await _userService.CreateUser(applicationUser, registrationRequest.Password);

            if (!result.IsSuccess)
                return BadRequest(result.ValidationErrors);
            return Ok(new { message = $"User {registrationRequest.Username} created successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await _authService.LoginAsync(loginRequest.Email, loginRequest.Password);

            if (!result.IsSuccess)
                return Unauthorized(result.ValidationErrors);

            return Ok(new { token = result.SuccessMessage });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _authService.GetCurrentUserAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(UserMapper.ToResponse(user));
        }
    }
}
