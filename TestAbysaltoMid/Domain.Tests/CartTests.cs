using AbySalto.Mid.Domain.Common;
using AbySalto.Mid.Domain.Entities;
using FluentAssertions;

namespace TestAbysaltoMid.Domain.Tests
{
    public class CartTests
    {
        [Fact]
        public void AddItem_Success()
        {
            var cart = new Cart();

            cart.AddItem(1, 2);

            cart.Items.Should().HaveCount(1);
            cart.Items.First().ProductId.Should().Be(1);
            cart.Items.First().Quantity.Should().Be(2);
        }

        [Fact]
        public void Increase_QuantityWhenAlreadyExists_Success()
        {
            var cart = new Cart();

            cart.AddItem(1, 2);
            cart.AddItem(1, 3);

            cart.Items.First().Quantity.Should().Be(5);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public void Increase_QuantityIsInvalid_Invalid(int productId, int quantity)
        {
            var cart = new Cart();

            var act = () => cart.AddItem(productId, quantity);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity must be bigger than 0");
        }

        [Fact]
        public void RemoveItem_ItemExist_Success()
        {
            var cart = new Cart();

            cart.AddItem(1, 2);
            cart.RemoveItem(1);

            cart.Items.Should().BeEmpty();
        }

        [Fact]
        public void RemovingItem_NonExistingItem_Invalid()
        {
            var cart = new Cart();

            var act = () => cart.RemoveItem(999);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Item not found");
        }
    }
}
