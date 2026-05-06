using AbySalto.Mid.Application.Product;
using AbySalto.Mid.Domain.Common;
using Ardalis.Result;
using Entity = AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Cart
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductGateway _productGateway;

        public CartService(
            ICartRepository cartRepository,
            IProductGateway productClient)
        {
            _cartRepository = cartRepository;
            _productGateway = productClient;
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return new CartDto { UserId = userId };

            return await Map(cart);
        }

        private async Task<CartDto> Map(Entity.Cart cart)
        {
            var dto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId
            };

            foreach (var item in cart.Items)
            {
                var product = await _productGateway.GetByIdAsync(item.ProductId);

                if (product != null)
                {
                    dto.Items.Add(new CartItemDto
                    {
                        ProductId = item.ProductId,
                        Title = product.Title,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            return dto;
        }
    }
}
