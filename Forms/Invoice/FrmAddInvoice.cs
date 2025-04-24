using Castle.Components.DictionaryAdapter.Xml;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

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
            DateTime today = DateTime.Today;
            if (!isEditMode)
            {
                Invoice invoice = new Invoice();
                if (txtDescription.Text == "" || txtDescription.Text == "")
                {
                    return;
                }
                int InvoiceNumber = int.Parse(txtInvoiceNumber.Text);
                string Description = txtDescription.Text;
                DateTime PaymentDeadline = dateTimePicker2.Value;
                DateTime InvoiceDate = dateTimePicker3.Value;
                invoice.Id = Guid.NewGuid();
                invoice.Date = today;
                invoice.Month =dateTimePicker3.Value.ToString("MMMM");
                invoice.PaymentDeadline = PaymentDeadline;
                invoice.Period = $"{new DateTime(today.Year, today.Month, 1):dd.MM.yyyy} - {new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1):dd.MM.yyyy}";
                invoice.InvoiceNumber = InvoiceNumber;
                invoice.InvoiceDate = InvoiceDate;
                invoice.Description = Description;
                service.Create(invoice);
                FrmInvoice.bindingSource1.Add(invoice);
                FrmInvoice.PopulateDataGridView();
                
            }
            else
            {
                currentInvoice.InvoiceNumber =int.Parse(txtInvoiceNumber.Text);
                currentInvoice.Description = txtDescription.Text;
                currentInvoice.PaymentDeadline = dateTimePicker2.Value;
                currentInvoice.InvoiceDate = dateTimePicker3.Value;
                currentInvoice.Period = $"{new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, 1):dd.MM.yyyy} - {new DateTime(dateTimePicker3.Value.Year, dateTimePicker3.Value.Month, 1).AddMonths(1).AddDays(-1):dd.MM.yyyy}";
                currentInvoice.Date = today;
                currentInvoice.Month =  dateTimePicker3.Value.ToString("MMMM");
                service.Update(currentInvoice);
                this.Close();
            }
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

        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            if (!Char.IsLetter(e.KeyChar) && !Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}