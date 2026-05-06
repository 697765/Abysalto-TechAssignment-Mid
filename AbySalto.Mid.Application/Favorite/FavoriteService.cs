using AbySalto.Mid.Application.Product;
using AbySalto.Mid.Domain.Common;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Domain.Interfaces;
using Ardalis.Result;

namespace AbySalto.Mid.Application.Favorite
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IProductGateway _productGateway;

        public FavoriteService(IFavoriteRepository repository, IProductGateway productService)
        {
            _favoriteRepository = repository;
            _productGateway = productService;
        }

        public async Task<Result> AddFavoriteAsync(string userId, int productId)
        {
            var favoriteExists = await _favoriteRepository.ExistsAsync(userId, productId);
            if (favoriteExists)
                return Result.Invalid(new ValidationError("Favorite already added"));

            var productExists = await _productGateway.GetByIdAsync(productId);
            if (productExists is null)
                return Result.Invalid(new ValidationError("Product doesn´t exists"));

            try
            {
                var favorite = new UserProductFavorite(userId, productId);

                await _favoriteRepository.AddAsync(favorite);
            }
            catch (DomainException ex)
            {
                return Result.Invalid(new ValidationError(ex.Message));
            }

            return Result.Success();
        }
    }
}
