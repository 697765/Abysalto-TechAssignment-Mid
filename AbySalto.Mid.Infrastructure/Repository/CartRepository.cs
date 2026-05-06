using AbySalto.Mid.Application.Cart;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;


namespace AbySalto.Mid.Infrastructure.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
