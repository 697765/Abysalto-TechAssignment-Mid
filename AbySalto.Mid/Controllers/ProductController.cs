using AbySalto.Mid.Application.Favorite;
using AbySalto.Mid.Application.Product;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AbySalto.Mid.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductGateway _productGateway;
        private readonly IFavoriteService _favoriteService;

        public ProductsController(IProductGateway gateway, IFavoriteService favoriteService)
        {
            _productGateway = gateway;
            _favoriteService = favoriteService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productGateway.GetAllAsync();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productGateway.GetByIdAsync(id);

            return product == null ? NotFound() : Ok(product);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("AddFavorite")]
        public async Task<IActionResult> Add(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _favoriteService.AddFavoriteAsync(userId, productId);

            if (!result.IsSuccess)
                return BadRequest(result.ValidationErrors);
            return Ok(new { message = $"Product {productId} was added to favourites" });
        }
    }
}
