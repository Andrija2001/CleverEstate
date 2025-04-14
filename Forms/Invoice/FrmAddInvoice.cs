using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.Clients;
using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CleverEstate.Forms.Invoices
{
    public partial class FrmAddInvoice : Form
    {
        InvoiceService service;
        FrmInvoice FrmInvoice;
        private Invoice currentInvoice;
        private bool isEditMode;
        public FrmAddInvoice(FrmInvoice FrmInvoice, InvoiceService service)
        {
            InitializeComponent();
            this.FrmInvoice = FrmInvoice;
            this.service = service;
        }
        public FrmAddInvoice(FrmInvoice parentForm, InvoiceService service, Invoice InvoiceToEdit)
      : this(parentForm, service)
        {
            this.currentInvoice = InvoiceToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditClients";
            button1.Text = "Edit Client";
            dateTimePicker2.Value = InvoiceToEdit.PaymentDeadline;
            txtInvoiceNumber.Text = InvoiceToEdit.InvoiceNumber.ToString();
            txtDescription.Text = InvoiceToEdit.Description;
            dateTimePicker3.Value= InvoiceToEdit.InvoiceDate;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtInvoiceNumber.Text, out int invoiceNumber))
            {
                return;
            }

            DateTime paymentDeadline = dateTimePicker2.Value;
            DateTime invoiceDate = dateTimePicker3.Value;
            string description = txtDescription.Text;

            if (string.IsNullOrWhiteSpace(description))
            {
                return;
            }

            if (isEditMode && currentInvoice != null)
            {
                currentInvoice.PaymentDeadline = paymentDeadline;
                currentInvoice.InvoiceNumber = invoiceNumber;
                currentInvoice.InvoiceDate = invoiceDate;
                currentInvoice.Description = description;
                service.Update(currentInvoice);
            }
            else
            {
                DateTime today = DateTime.Today;
                Invoice invoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    Date = today,
                    Month = today.ToString("MMMM"),
                    Period = $"{new DateTime(today.Year, today.Month, 1):dd.MM.yyyy} - {new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1):dd.MM.yyyy}",
                    PaymentDeadline = paymentDeadline,
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = invoiceDate,
                    Description = description
                };
                service.Create(invoice);
            }
            this.Hide();
            FrmInvoice.LoadInvoices();
        }
        private void txtInvoiceNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}