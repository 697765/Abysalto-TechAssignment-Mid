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

        public async Task<Result<CartDto>> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var product = await _productGateway.GetByIdAsync(request.ProductId);

            if (product == null)
                return Result.Invalid(new ValidationError("Product not found"));

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            var existingQuantity = cart?.Items.FirstOrDefault(x => x.ProductId == request.ProductId)?.Quantity ?? 0;
            var totalQuantity = request.Quantity + existingQuantity;
            if (product.Stock < totalQuantity)
                return Result.Invalid(new ValidationError($"There is {product.Stock - existingQuantity} products on stock"));

            if (cart == null)
                cart = Entity.Cart.Create(userId);

            try
            {
                cart.AddItem(request.ProductId, request.Quantity);
            }
            catch (DomainException ex)
            {
                return Result.Invalid(new ValidationError(ex.Message));
            }

            await _cartRepository.SaveAsync(cart);

            return await Map(cart);
        }

        public async Task<Result<CartDto>> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
                return Result.Invalid(new ValidationError("Cart not found"));

            try
            {
                cart.RemoveItem(productId);
            }
            catch (DomainException ex)
            {
                return Result.Invalid(new ValidationError(ex.Message));
            }

            await _cartRepository.SaveAsync(cart);

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
