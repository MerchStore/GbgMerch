using GbgMerch.Application.DTOs;
using GbgMerch.Application.Services.Interfaces;
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
    private readonly ICatalogService _catalogService;
    

    public BasicProductsApiController(IProductRepository productRepository, ICatalogService catalogService)
    {
        _productRepository = productRepository;
        _catalogService = catalogService;
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
                StockQuantity = p.StockQuantity,
                Category = p.Category,      // ✅ Lägg till detta
                Tags = p.Tags   
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
                StockQuantity = product.StockQuantity,
                Category = product.Category,          // ✅ Lägg till
                Tags = product.Tags     
            };

            return Ok(dto);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving the product" });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var newProductId = await _catalogService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newProductId }, new { id = newProductId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Ett fel inträffade när produkten skulle skapas.",
                error = ex.Message
            });
        }
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var success = await _catalogService.UpdateProductAsync(id, dto);

            if (!success)
                return NotFound(new { message = $"Produkten med ID {id} hittades inte." });

            return NoContent(); // 204 = uppdatering lyckades, inget innehåll behövs tillbaka
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Ett fel inträffade vid uppdatering.",
                error = ex.Message
            });
        }
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var success = await _catalogService.DeleteProductAsync(id);

            if (!success)
                return NotFound(new { message = $"Produkten med ID {id} hittades inte." });

            return NoContent(); // 204 = borttagen
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Ett fel inträffade vid borttagning.",
                error = ex.Message
            });
        }
    }
}
