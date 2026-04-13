using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BillingService.Data;
using BillingService.Models;
using BillingService.DTOs;
using BillingService.Requests;

namespace BillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public InvoicesController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            return invoice;
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> CreateInvoice()
        {
            var invoice = new Invoice
            {
                Number = GenerateInvoiceNumber(),
                Status = "Open",
                Items = new List<InvoiceItem>()
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
        }

        private int GenerateInvoiceNumber()
        {
            return _context.Invoices.Any()
                ? _context.Invoices.Max(i => i.Number) + 1
                : 1;
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItem(int id, AddItemRequest request)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound("Invoice not found");

            if (invoice.Status != "Open")
                return BadRequest("Cannot add items to a closed invoice");

            var response = await _httpClient.GetAsync(
                $"https://localhost:7076/api/products/{request.ProductId}");

            if (!response.IsSuccessStatusCode)
                return NotFound("Product not found");

            var item = new InvoiceItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                InvoiceId = invoice.Id
            };

            invoice.Items.Add(item);

            await _context.SaveChangesAsync();

            return Ok(invoice);
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound("Invoice not found");

            if (invoice.Status != "Open")
                return BadRequest("Invoice already closed");

            foreach (var item in invoice.Items)
            {
                var response = await _httpClient.PutAsJsonAsync(
                    $"https://localhost:7076/api/products/{item.ProductId}/decrease",
                    new { quantity = item.Quantity });

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Stock service error: {errorMessage}");
                }
            }

            invoice.Status = "Closed";

            await _context.SaveChangesAsync();
            return Ok(invoice);
        }

        [HttpPut("{invoiceId}/items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int invoiceId, int itemId, UpdateItemRequest request)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                return NotFound("Invoice not found");

            if (invoice.Status != "Open")
                return BadRequest("Cannot update items of a closed invoice");

            var item = invoice.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                return NotFound("Item not found");

            if (request.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero");

            item.Quantity = request.Quantity;

            await _context.SaveChangesAsync();

            return Ok(invoice);
        }

        [HttpDelete("{invoiceId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int invoiceId, int itemId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                return NotFound("Invoice not found");

            if (invoice.Status != "Open")
                return BadRequest("Cannot remove items from a closed invoice");

            var item = invoice.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                return NotFound("Item not found");

            _context.InvoiceItems.Remove(item);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}