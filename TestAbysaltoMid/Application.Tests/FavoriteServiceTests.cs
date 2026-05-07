using AbySalto.Mid.Application.Favorite;
using AbySalto.Mid.Application.Product;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace TestAbysaltoMid.Application.Tests
{
    public class FavoriteServiceTests
    {
        private readonly Mock<IFavoriteRepository> _repo = new();
        private readonly Mock<IProductGateway> _gateway = new();
        private readonly FavoriteService _service;

        public FavoriteServiceTests()
        {
            _service = new FavoriteService(_repo.Object, _gateway.Object);
        }

        [Fact]
        public async Task AddFavoriteAsync_FavoriteAlreadyExist_Invalid()
        {
            _repo.Setup(x => x.ExistsAsync("user1", 1))
                .ReturnsAsync(true);

            var result = await _service.AddFavoriteAsync("user1", 1);

            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors
                .First()
                .ErrorMessage
                .Should()
                .Be("Favorite already added");
        }

        [Fact]
        public async Task AddFavoriteAsync_ProductNotFound_Invalid()
        {
            _repo.Setup(x => x.ExistsAsync("user1", 1))
                .ReturnsAsync(false);
            _gateway.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((ProductDto)null);

            var result = await _service.AddFavoriteAsync("user1", 1);

            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors
                .First()
                .ErrorMessage
                .Should()
                .Be("Product doesn´t exists");
        }

        [Fact]
        public async Task AddFavoriteAsync_Successfully()
        {
            _repo.Setup(x => x.ExistsAsync("user1", 1))
                .ReturnsAsync(false);
            _gateway.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new ProductDto { Id = 1, Title = "Test", Price = 10 });

            var result = await _service.AddFavoriteAsync("user1", 1);

            result.IsSuccess.Should().BeTrue();

            _repo.Verify(x => x.AddAsync(It.IsAny<UserProductFavorite>()), Times.Once);
        }
    }
}
