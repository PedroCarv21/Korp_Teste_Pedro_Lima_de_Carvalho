using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BillingService.Data;
using BillingService.Models;
using BillingService.DTOs;

namespace BillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
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
    }
}