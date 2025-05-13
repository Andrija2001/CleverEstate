using CleverEstate.Models;
using CleverEstate.Services.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Classes.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DataDbContext _context;
        public InvoiceRepository(DataDbContext context)
        {
            _context = context;
        }
        public void Delete(Guid InvoiceId)
        {
            var InvoiceInDb = _context.Invoices.Find(InvoiceId);
            if (InvoiceInDb != null)
            {
                _context.Invoices.Remove(InvoiceInDb);
                _context.SaveChanges();
            }
        }

        public List<Invoice> GetAll()
        {
            return _context.Invoices.ToList();
        }
        public Invoice GetById(Guid id)
        {
            return _context.Invoices.Find(id);
        }
        public void Insert(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(Invoice invoice)
        {
            var existingEntity = _context.Invoices
                         .SingleOrDefault(i => i.Id == invoice.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = System.Data.Entity.EntityState.Detached;
            }
            _context.Entry(invoice).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}