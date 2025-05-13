using CleverEstate.Models;
using CleverEstate.Services.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Classes.Repository
{
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private readonly DataDbContext _context;
        public InvoiceItemRepository(DataDbContext context)
        {
            _context = context;
        }
        public void Delete(Guid InvoiceItemId)
        {
            var InvoiceItemInDb = _context.InvoiceItems.Find(InvoiceItemId);
            if (InvoiceItemInDb != null)
            {
                _context.InvoiceItems.Remove(InvoiceItemInDb);
                _context.SaveChanges();
            }
        }
        public List<InvoiceItem> GetAll()
        {
            return _context.InvoiceItems.ToList();
        }
        public InvoiceItem GetById(Guid id)
        {
            return _context.InvoiceItems.Find(id);
        }
        public void Insert(InvoiceItem invoiceItem)
        {
            _context.InvoiceItems.Add(invoiceItem);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(InvoiceItem invoiceItem)
        {
            var existingEntity = _context.InvoiceItems
                       .SingleOrDefault(inv => inv.Id == invoiceItem.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = System.Data.Entity.EntityState.Detached;
            }
            _context.Entry(invoiceItem).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}