using Ardalis.Result;

namespace AbySalto.Mid.Application.Cart
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string userId);
    }
}
