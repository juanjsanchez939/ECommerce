using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    // GET: api/product
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _repository.GetAllAsync();

        return Ok(products);
    }

    // GET: api/product/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    // POST: api/product
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId);

        await _repository.AddAsync(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    // DELETE: api/product/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);

        return NoContent();
    }
}

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId);