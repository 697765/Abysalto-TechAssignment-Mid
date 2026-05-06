using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<bool> ExistsAsync(string userId, int productId);
        Task AddAsync(UserProductFavorite favorite);
    }
}
