using AbySalto.Mid.Domain.Entities;
using Ardalis.Result;

namespace AbySalto.Mid.Application.Authorization
{
    public interface IAuthorizationService
    {
        Task<Result> LoginAsync(string email, string password);
        Task<ApplicationUser?> GetCurrentUserAsync(string userId);
    }
}
