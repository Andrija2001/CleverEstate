using CleverState.Services.Interface;
using CleverEstate.Models;
using System.Collections.Generic;
using System;

namespace CleverState.Services.Classes
{
    public class InvoiceItemService : IInvoiceItemServices
    {
        private List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
        public void Create(InvoiceItem invoiceItem)
        {
            invoiceItems.Add(invoiceItem);

        }

        public void Delete(Guid id)
        {
            var RemoveInvoiceItem = invoiceItems.Find(a => a.Id == id);
            if (RemoveInvoiceItem != null)
            {
                invoiceItems.Remove(RemoveInvoiceItem);
            }
        }

        public InvoiceItem GetInvoiceItem(Guid id)
        {
            return invoiceItems.Find(a => a.Id == id);
        }

     
        public void Update(InvoiceItem InvoiceItem)
        {
            var UpdateInvoiceItems = invoiceItems.Find(a => a.Id == a.Id);
            if (UpdateInvoiceItems != null)
            {
                UpdateInvoiceItems.Id = InvoiceItem.Id;
                UpdateInvoiceItems.InvoiceId = InvoiceItem.InvoiceId;
                UpdateInvoiceItems.Quantity = InvoiceItem.Quantity;
                UpdateInvoiceItems.Number = InvoiceItem.Number;
                UpdateInvoiceItems.PricePerUnit = InvoiceItem.PricePerUnit;
                UpdateInvoiceItems.TotalPrice = InvoiceItem.TotalPrice;
                UpdateInvoiceItems.VATRate = InvoiceItem.VATRate;
                UpdateInvoiceItems.ItemCatalogId = InvoiceItem.ItemCatalogId;
                UpdateInvoiceItems.VAT = InvoiceItem.VAT;
            }
        }

        public List<InvoiceItem> GetAllInvoiceItems()
        {
            return invoiceItems;
        }
    }
}
