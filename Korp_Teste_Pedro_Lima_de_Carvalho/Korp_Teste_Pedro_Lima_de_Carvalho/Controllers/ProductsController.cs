using Korp_Teste_Pedro_Lima_de_Carvalho.Data;
using Korp_Teste_Pedro_Lima_de_Carvalho.Models;
using Korp_Teste_Pedro_Lima_de_Carvalho.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Korp_Teste_Pedro_Lima_de_Carvalho.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest("ID mismatch");

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/decrease")]
        public async Task<IActionResult> DecreaseStock(int id, [FromBody] DecreaseStockRequest request)
        {
            if (request.Quantity == 999)
            {
                return StatusCode(500, "Simulated stock service failure");
            }

            if (request.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero");
            }

            var productExists = await _context.Products.AnyAsync(p => p.Id == id);

            if (!productExists)
            {
                return NotFound("Product not found");
            }

            var affectedRows = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Products SET Stock = Stock - {0} WHERE Id = {1} AND Stock >= {0}",
                request.Quantity, id);

            if (affectedRows == 0)
            {
                return BadRequest("Insufficient stock");
            }

            var updatedProduct = await _context.Products.FindAsync(id);

            return Ok(updatedProduct);
        }
    }
}