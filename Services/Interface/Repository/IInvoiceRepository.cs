using CleverEstate.Models;
using System;
using System.Collections.Generic;
namespace CleverEstate.Services.Interface.Repository
{
    public interface IInvoiceRepository
    {
        List<Invoice> GetAll();
        Invoice GetById(Guid id);
        void Insert(Invoice invoice);
        void Update(Invoice invoice);
        void Delete(Guid InvoiceId);
        void Save();
    }
}