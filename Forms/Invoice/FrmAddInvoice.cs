using Castle.Components.DictionaryAdapter.Xml;
using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.Clients;
using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface.Repository;
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
        private readonly InvoiceRepository invoiceRepository;
        private readonly ClientRepository clientRepository;

        private readonly FrmInvoice parentForm;
        private readonly Invoice currentInvoice;
        private readonly bool isEditMode;

        public FrmAddInvoice(FrmInvoice parentForm, InvoiceRepository invoiceRepository, ClientRepository clientRepository)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.invoiceRepository = invoiceRepository;
            this.currentInvoice = new Invoice();
            this.isEditMode = false;
            this.clientRepository = clientRepository;
        }
        public FrmAddInvoice(FrmInvoice parentForm, InvoiceRepository invoiceRepository, ClientRepository clientRepository, Invoice InvoiceToEdit)
      : this(parentForm, invoiceRepository, clientRepository)
        {
            this.currentInvoice = InvoiceToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditClients";
            button1.Text = "Edit Client";
            DateTimePickerPaymentDeadline.Value = InvoiceToEdit.PaymentDeadline;
            txtInvoiceNumber.Text = InvoiceToEdit.InvoiceNumber.ToString();
            txtDescription.Text = InvoiceToEdit.Description;
            DateTimePickerInvoiceDate.Value= InvoiceToEdit.InvoiceDate;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime date = dateTimerPickerDate.Value;
            string month = cmbMonth.SelectedItem.ToString();
            DateTime paymentDeadline = DateTimePickerPaymentDeadline.Value;
            string period = txtPeriod.Text;
            int invoicenumber = int.Parse(txtInvoiceNumber.Text);
            DateTime invoiceDate = DateTimePickerInvoiceDate.Value;
            string description = txtDescription.Text;

            if (string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtInvoiceNumber.Text))
            {
                MessageBox.Show("Unesite i adresu i grad.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ako se nalazi u edit modu, ažuriraj postojeći invoice
            if (isEditMode)
            {
                currentInvoice.Date = date;
                currentInvoice.Month = month;
                currentInvoice.PaymentDeadline = paymentDeadline;
                currentInvoice.Period = period;
                currentInvoice.InvoiceNumber = invoicenumber;
                currentInvoice.Description = description;
                currentInvoice.InvoiceDate = invoiceDate;
            }
            else
            {
                // Uzmi prvi klijentov ID iz repozitorijuma
                var clientId = clientRepository.GetAll().FirstOrDefault()?.Id ?? Guid.Empty;

                var newInvoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    Description = description,
                    InvoiceDate = invoiceDate,
                    InvoiceNumber = invoicenumber,
                    Month = month,
                    PaymentDeadline = paymentDeadline,
                    Period = period,
                    ClientId = clientId // Koristi uzeti ClientId
                };

                invoiceRepository.Insert(newInvoice);
                parentForm.bindingSource1.Add(newInvoice);
            }

            parentForm.bindingSource1.ResetBindings(false);
            this.DialogResult = DialogResult.OK;
            this.Close();
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