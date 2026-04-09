using Microsoft.EntityFrameworkCore;
using BillingService.Models;

namespace BillingService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}