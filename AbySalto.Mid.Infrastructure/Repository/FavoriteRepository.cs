using AbySalto.Mid.Domain.Common;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Domain.Interfaces;
using AbySalto.Mid.Infrastructure.Persistance;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<bool> ExistsAsync(string userId, int productId)
        {
            return _context.UserProductFavorites.AnyAsync(x => x.UserId == userId && x.ProductId == productId);
        }

        public Task AddAsync(UserProductFavorite favorite)
        {
            _context.UserProductFavorites.Add(favorite);
            return _context.SaveChangesAsync();
        }
    }
}
