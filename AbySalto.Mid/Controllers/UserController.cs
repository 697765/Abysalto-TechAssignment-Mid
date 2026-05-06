using AbySalto.Mid.Application.User;
using AbySalto.Mid.WebApi.Mappers;
using AbySalto.Mid.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Auth = AbySalto.Mid.Application.Authorization;

namespace AbySalto.Mid.WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Auth.IAuthorizationService _authService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
    }
}
