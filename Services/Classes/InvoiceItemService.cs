using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;
using CleverEstate.Services.Interface;
using CleverEstate.Services.Interface.Repository;

namespace CleverState.Services.Classes
{
    public class InvoiceItemService : IInvoiceItemServices
    {
        private readonly IInvoiceItemRepository _repository;
        public InvoiceItemService(IInvoiceItemRepository repository)
        {
            _repository = repository;
        }

        public void Create(InvoiceItem invoiceItem) => _repository.Insert(invoiceItem);

        public void Delete(Guid id) => _repository.Delete(id);

        public InvoiceItem GetInvoiceItem(Guid id) => _repository.GetById(id);

        public void Update(InvoiceItem invoiceItem) => _repository.Update(invoiceItem);

        public List<InvoiceItem> GetInvoiceItems() => _repository.GetAll();
    }
}
