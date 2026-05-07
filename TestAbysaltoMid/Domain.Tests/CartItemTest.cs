using AbySalto.Mid.Domain.Common;
using AbySalto.Mid.Domain.Entities;
using FluentAssertions;

namespace TestAbysaltoMid.Domain.Tests
{
    public class CartItemTest
    {

        [Fact]
        public void CreateCartItem_ValidValues_Success()
        {
            var item = new CartItem(1, 2);

            item.ProductId.Should().Be(1);
            item.Quantity.Should().Be(2);
        }

        [Fact]
        public void Increase_ValidValues_Success()
        {
            var item = new CartItem(1, 2);

            item.Increase(3);

            item.Quantity.Should().Be(5);
        }

        [Fact]
        public void Increase_NegativeValue_Invalid()
        {
            var item = new CartItem(1, 5);

            var act = () => item.Increase(-2);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity must be greater than 0");
        }
    }
}
