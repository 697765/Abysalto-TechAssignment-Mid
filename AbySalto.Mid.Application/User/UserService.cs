using AbySalto.Mid.Application.User;
using AbySalto.Mid.Domain.Entities;
using Ardalis.Result;
using Microsoft.AspNetCore.Identity;

internal class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> CreateUser(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return Result.Invalid(result.Errors
                .Select(e => new ValidationError(e.Description))
                .ToList());
        }

        return Result.Success();
    }
}