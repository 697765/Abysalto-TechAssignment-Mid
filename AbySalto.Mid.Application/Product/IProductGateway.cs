namespace AbySalto.Mid.Application.Product
{
    public interface IProductGateway
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
    }
}
