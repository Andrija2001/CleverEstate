using System;
using System.Collections.Generic;

namespace CleverEstate.Models
{
    public class ItemCatalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public int Unit { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
