using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Korp_Teste_Pedro_Lima_de_Carvalho.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        public int Stock { get; set; }
    }
}