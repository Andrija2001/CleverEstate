using System;
using System.Collections.Generic;

namespace CleverEstate.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Month { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public string Period { get; set; }
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Description { get; set; }
        public Guid ClientId { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
