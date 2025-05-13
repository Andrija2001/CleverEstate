using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;
using CleverEstate.Services.Interface.Repository;

namespace CleverState.Services.Classes
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        public InvoiceService(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public void Create(Invoice invoice) => _repository.Insert(invoice);

        public void Delete(Guid id) => _repository.Delete(id);

        public Invoice GetInvoice(Guid id) => _repository.GetById(id);

        public void Update(Invoice invoice) => _repository.Update(invoice);

        public List<Invoice> GetInvoice() => _repository.GetAll();
    }
}
