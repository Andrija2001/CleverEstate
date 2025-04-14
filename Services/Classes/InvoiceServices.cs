using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;

namespace CleverState.Services.Classes
{
    public class InvoiceService : IInvoiceService
    {
        private List<Invoice> Invoices = new List<Invoice>();

        public void Create(Invoice invoice)
        {
            Invoices.Add(invoice);

        }

        public void Delete(Guid Id)
        {
            var RemoveInvoice = Invoices.Find(a => a.Id == Id);
            if (RemoveInvoice != null)
            {
                Invoices.Remove(RemoveInvoice);
            }
        }

        public Invoice GetInvoice(Guid Id)
        {
            return Invoices.Find(a => a.Id == Id);
        }

        public void Update(Invoice Invoice)
        {
            var UpdateInvoice = Invoices.Find(a => a.Id == a.Id);
            if (UpdateInvoice != null)
            {
                UpdateInvoice.Id = Invoice.Id;
                UpdateInvoice.InvoiceNumber = Invoice.InvoiceNumber;
                UpdateInvoice.InvoiceDate = Invoice.InvoiceDate;
                UpdateInvoice.Date = Invoice.Date;
                UpdateInvoice.Period = Invoice.Period;
                UpdateInvoice.Description = Invoice.Description;
                UpdateInvoice.Month = Invoice.Month;
                UpdateInvoice.PaymentDeadline = Invoice.PaymentDeadline;
            }
        }


        public List<Invoice> GetAllInvoices()
        {
            return Invoices;
        }
    }
}
