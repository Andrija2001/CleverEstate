using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Models
{
    public class DataDbContext : DbContext
    {
        public DataDbContext() : base("name=Data")
        {

        }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet <Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<ItemCatalog> ItemCatalogs { get; set; }
      

    }
}
