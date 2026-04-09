using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BillingService.Data;
using BillingService.Models;

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
    }
}