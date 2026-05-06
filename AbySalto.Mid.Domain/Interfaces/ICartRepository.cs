using Entity = AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Cart
{
    public interface ICartRepository
    {
        Task<Entity.Cart?> GetByUserIdAsync(string userId);
    }
}
