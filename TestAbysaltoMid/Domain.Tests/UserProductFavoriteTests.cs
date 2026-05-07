using AbySalto.Mid.Domain.Common;
using AbySalto.Mid.Domain.Entities;
using FluentAssertions;

namespace TestAbysaltoMid.Domain.Tests
{
    public class UserProductFavoriteTests
    {
        [Fact]
        public void Create_ValidData_Success()
        {
            var favorite = new UserProductFavorite("user1", 1);

            favorite.UserId.Should().Be("user1");
            favorite.ProductId.Should().Be(1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_EmptyUserId_Invalid(string userId)
        {
            Action act = () => new UserProductFavorite(userId, 1);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("UserId is required");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Create_InvalidkProductId_Invalid(int productId)
        {
            Action act = () => new UserProductFavorite("user1", productId);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Invalid productId");
        }
    }
}
