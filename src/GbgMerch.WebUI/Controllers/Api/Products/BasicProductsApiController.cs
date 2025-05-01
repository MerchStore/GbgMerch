using Microsoft.AspNetCore.Mvc;
using GbgMerch.Domain.Interfaces;
using GbgMerch.WebUI.Controllers.Api.Products;
using GbgMerch.WebUI.ViewModels.Api.Basic;
using Microsoft.AspNetCore.Authorization; // ✅ Nödvändigt för [Authorize]


namespace GbgMerch.WebUI.Controllers.Api.Products;

[Authorize(Policy = "ApiKeyPolicy")] 
[ApiController]
[Route("api/basic/products")]
public class BasicProductsApiController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    

    public BasicProductsApiController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BasicProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();

            var dtoList = products.Select(p => new BasicProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price.Amount,
                Currency = p.Price.Currency,
                ImageUrl = p.ImageUrl?.ToString(),
                StockQuantity = p.StockQuantity
            });

            return Ok(dtoList);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving products" });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BasicProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return NotFound(new { message = $"Product with ID {id} not found" });

            var dto = new BasicProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.Amount,
                Currency = product.Price.Currency,
                ImageUrl = product.ImageUrl?.ToString(),
                StockQuantity = product.StockQuantity
            };

            return Ok(dto);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving the product" });
        }
    }
}
