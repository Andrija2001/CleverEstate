using System;
using System.Collections.Generic;

namespace CleverEstate.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public int PIB { get; set; }
        public string BankAccount { get; set; }
        public Guid InvoiceId { get; set; }

        public List<Apartment> Apartments { get; set; }
        public List<Invoice> Invoices { get; set; }
        public override string ToString()
        {
            return Name + " " + Surname;  // Spajanje imena i prezimena
        }
    }
}
