using CleverEstate.Models;
using System;

namespace CleverState.Services.Interface
{
    internal interface IInvoiceItemServices
    {
        void Create(InvoiceItem obj);
        InvoiceItem GetInvoiceItem(Guid Id);
        void Update(InvoiceItem InvoiceItem);
        void Delete(Guid Id);
    }
}
