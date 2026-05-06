using AbySalto.Mid.Application.Product;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace AbySalto.Mid.Infrastructure.Services.Products
{
    public class ProductGateway : IProductGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        private const string CacheKey = "all_products";
        private const string Endpoint = "products";

        public ProductGateway(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var response = await GetCachedProductsAsync();

            return response.Products
                .Select(x => Map(x))
                .ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int productId)
        {
            var response = await GetCachedProductsAsync();

            var product = response.Products.FirstOrDefault(p => p.Id == productId);

            return product == null ? null : Map(product);
        }

        private async Task<ProductResponse> GetCachedProductsAsync() =>
            await _cache.GetOrCreateAsync(CacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);

                return await _httpClient.GetFromJsonAsync<ProductResponse>(Endpoint);
            }) ?? new();

        private static ProductDto Map(ProductApiModel x)
            => new()
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Stock = x.Stock
            };
    }
}
