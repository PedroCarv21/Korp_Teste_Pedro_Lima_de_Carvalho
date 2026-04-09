using Microsoft.EntityFrameworkCore;
using Korp_Teste_Pedro_Lima_de_Carvalho.Models;

namespace Korp_Teste_Pedro_Lima_de_Carvalho.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}