using CleverEstate.Models;
using System;

namespace CleverState.Services.Interface
{
    internal interface IInvoiceService
    {
        void Create(Invoice obj);
        Invoice GetInvoice(Guid id);
        void Update(Invoice invoices);
        void Delete(Guid id);
    }
}
