using System;

namespace CleverEstate.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int PIB { get; set; }
        public string BankAccount { get; set; }
        public Guid InvoiceId { get; set; }
    }
}
