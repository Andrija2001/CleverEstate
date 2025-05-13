using CleverEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleverEstate.Services.Interface.Repository
{
    public interface IInvoiceItemRepository
    {
        List<InvoiceItem> GetAll();
        InvoiceItem GetById(Guid id);
        void Insert(InvoiceItem invoiceItem);
        void Update(InvoiceItem invoiceItem);
        void Delete(Guid InvoiceItemId);
        void Save();
    }
}