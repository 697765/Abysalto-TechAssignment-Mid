using AbySalto.Mid.Domain.Common;

namespace AbySalto.Mid.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items;

        public static Cart Create(string userId)
        {
            return new Cart
            {
                UserId = userId
            };
        }

        public void AddItem(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity must be bigger than 0");

            var item = _items.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                _items.Add(new CartItem(productId, quantity));
            }
            else
            {
                item.Increase(quantity);
            }
        }

        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
                throw new DomainException("Item not found");

            _items.Remove(item);
        }
    }
}
