using AbySalto.Mid.Domain.Common;

namespace AbySalto.Mid.Domain.Entities
{
    public class UserProductFavorite
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }

        public UserProductFavorite(string userId, int productId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new DomainException("UserId is required");

            if (productId <= 0)
                throw new DomainException("Invalid productId");

            UserId = userId;
            ProductId = productId;
        }
    }
}
