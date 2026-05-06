using AbySalto.Mid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbySalto.Mid.Infrastructure.Persistance
{
    public class UserProductFavoriteConfiguration : IEntityTypeConfiguration<UserProductFavorite>
    {
        public void Configure(EntityTypeBuilder<UserProductFavorite> builder)
        {
            builder.HasKey(x => new { x.UserId, x.ProductId });
        }
    }
}
