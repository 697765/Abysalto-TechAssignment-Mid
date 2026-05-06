using AbySalto.Mid.Domain.Entities;
using Ardalis.Result;

namespace AbySalto.Mid.Application.User
{
    public interface IUserService
    {
        Task<Result> CreateUser(ApplicationUser user, string password);
    }
}
