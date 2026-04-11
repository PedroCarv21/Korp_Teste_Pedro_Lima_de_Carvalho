using System.ComponentModel.DataAnnotations;

namespace Korp_Teste_Pedro_Lima_de_Carvalho.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}