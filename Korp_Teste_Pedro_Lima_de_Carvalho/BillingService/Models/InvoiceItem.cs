namespace BillingService.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; } // vem do outro microserviço
        public int Quantity { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}