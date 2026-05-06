using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Authorization
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}
