using System;

namespace CleverEstate.Models
{
    public class InvoiceItem
    {
        public Guid Id { get; set; }
        public Guid ItemCatalogId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal VAT { get; set; }
        public decimal VATRate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Number { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
