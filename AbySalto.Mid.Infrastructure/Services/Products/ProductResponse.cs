namespace AbySalto.Mid.Infrastructure.Services.Products
{
    internal class ProductResponse
    {
        public List<ProductApiModel> Products { get; set; } = new();
    }

    public class ProductApiModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
