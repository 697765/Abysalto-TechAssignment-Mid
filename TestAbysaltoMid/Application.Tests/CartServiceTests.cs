using AbySalto.Mid.Application.Cart;
using AbySalto.Mid.Application.Product;
using AbySalto.Mid.Domain.Entities;
using FluentAssertions;
using Moq;

namespace TestAbysaltoMid.Application.Tests
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _repo = new();
        private readonly Mock<IProductGateway> _gateway = new();
        private readonly CartService _service;

        public CartServiceTests()
        {
            _service = new CartService(_repo.Object, _gateway.Object);
        }

        [Fact]
        public async Task AddToCartAsync_Success()
        {
            _gateway.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new ProductDto { Id = 1, Stock = 5 });

            _repo.Setup(x => x.GetByUserIdAsync("user1"))
                .ReturnsAsync((Cart)null);

            var result = await _service.AddToCartAsync("user1",
                new AddToCartRequest { ProductId = 1, Quantity = 2 });

            result.IsSuccess.Should().BeTrue();
        }


        [Fact]
        public async Task AddToCartAsync_ProductDoesntExist_Invalid()
        {
            _gateway.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((ProductDto)null);

            var result = await _service.AddToCartAsync("user1",
                new AddToCartRequest { ProductId = 1, Quantity = 2 });

            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors
                .First()
                .ErrorMessage
                .Should()
                .Be("Product not found");
        }

        [Fact]
        public async Task AddToCartAsync_StockIsNotSufficient_Invalid()
        {
            var existingCart = new Cart();
            existingCart.AddItem(1, 3);

            _repo.Setup(x => x.GetByUserIdAsync("user1"))
                .ReturnsAsync(existingCart);

            _gateway.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new ProductDto
                {
                    Id = 1,
                    Stock = 5
                });

            var request = new AddToCartRequest
            {
                ProductId = 1,
                Quantity = 3
            };

            var result = await _service.AddToCartAsync("user1", request);

            result.IsSuccess.Should().BeFalse();

            result.ValidationErrors
                .First()
                .ErrorMessage
                .Should()
                .Be("There is 2 products on stock");
        }

        [Fact]
        public async Task RemoveFromCartAsync_WhenCartNotFound_Invalid()
        {
            _repo.Setup(x => x.GetByUserIdAsync("user1"))
                .ReturnsAsync((Cart)null);

            var result = await _service.RemoveFromCartAsync("user1", 1);

            result.IsSuccess.Should().BeFalse();

            result.ValidationErrors
                .First()
                .ErrorMessage
                .Should()
                .Be("Cart not found");
        }

        [Fact]
        public async Task RemoveFromCartAsync_ItemNotFound_Invalid()
        {
            var cart = new Cart();

            _repo.Setup(x => x.GetByUserIdAsync("user1"))
                .ReturnsAsync(cart);

            var result = await _service.RemoveFromCartAsync("user1", 999);

            result.IsSuccess.Should().BeFalse();

            result.ValidationErrors
                .First()
                .ErrorMessage
                .Should()
                .Be("Item not found");
        }

        [Fact]
        public async Task RemoveFromCartAsync_Successfully()
        {
            var cart = new Cart();
            cart.AddItem(1, 2);

            _repo.Setup(x => x.GetByUserIdAsync("user1"))
                .ReturnsAsync(cart);

            var result = await _service.RemoveFromCartAsync("user1", 1);

            result.IsSuccess.Should().BeTrue();

            _repo.Verify(x => x.SaveAsync(cart), Times.Once);

            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCartAsync_CartNotFound_EmptyCart()
        {
            _repo.Setup(x => x.GetByUserIdAsync("user1"))
                .ReturnsAsync((Cart)null);

            var result = await _service.GetCartAsync("user1");

            result.UserId.Should().Be("user1");
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCartAsync_Success()
        {
            var cart = new Cart();
            cart.UserId = "user1";
            cart.AddItem(1, 2);

            _repo.Setup(x => x.GetByUserIdAsync("user1"))
                .ReturnsAsync(cart);

            _gateway.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new ProductDto
                {
                    Id = 1,
                    Title = "Test product",
                    Price = 10,
                    Stock = 2
                });

            var result = await _service.GetCartAsync("user1");

            result.UserId.Should().Be("user1");
            result.Items.Should().HaveCount(1);

            result.Items.First().ProductId.Should().Be(1);
            result.Items.First().Quantity.Should().Be(2);
        }
    }
}
