using AbySalto.Mid.Domain.Common;

namespace AbySalto.Mid.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public CartItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public void Increase(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than 0");

            Quantity += quantity;
        }
    }
}
