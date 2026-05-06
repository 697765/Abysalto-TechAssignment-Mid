using Ardalis.Result;

namespace AbySalto.Mid.Application.Favorite
{
    public interface IFavoriteService
    {
        Task<Result> AddFavoriteAsync(string userId, int productId);
    }
}
