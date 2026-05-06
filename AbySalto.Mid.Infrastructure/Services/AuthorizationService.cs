using AbySalto.Mid.Application.Authorization;
using AbySalto.Mid.Domain.Entities;
using Ardalis.Result;
using Microsoft.AspNetCore.Identity;

namespace AbySalto.Mid.Infrastructure.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthorizationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<Result> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result.Invalid(new ValidationError("Invalid credentials"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                return Result.Invalid(new ValidationError("Invalid credentials"));

            var token = _jwtService.GenerateToken(user);

            return Result.SuccessWithMessage(token);
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            return new ApplicationUser
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname
            };
        }
    }
}
