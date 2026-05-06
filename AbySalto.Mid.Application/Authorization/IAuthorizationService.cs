using Ardalis.Result;

namespace AbySalto.Mid.Application.Authorization
{
    public interface IAuthorizationService
    {
        Task<Result> LoginAsync(string email, string password);
    }
}
