using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Windows.Forms;
namespace CleverEstate.Forms.InvoiceItems
{
    public partial class FrmAddInvoiceItem : Form
    {
        InvoiceItemService service;
        FrmInvoiceItem FrmInvoiceItem;
        private InvoiceItem CurrentInvoiceItem;
        private bool isEditMode;
        public FrmAddInvoiceItem(FrmInvoiceItem parentForm, InvoiceItemService service, InvoiceItem InvoiceItemToEdit)
        : this(parentForm, service)
        {
            this.CurrentInvoiceItem = InvoiceItemToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditInvoiceItem";
            button1.Text = "Edit Invoice Item";
            txtNumber.Text = InvoiceItemToEdit.Number;
            txtPricePerUnit.Text=InvoiceItemToEdit.PricePerUnit.ToString();
            txtQuantity.Text=InvoiceItemToEdit.Quantity.ToString();
            txtTotalPrice.Text=InvoiceItemToEdit.TotalPrice.ToString();
            txtVAT.Text=InvoiceItemToEdit.VAT.ToString();
            txtVATRate.Text = InvoiceItemToEdit.VATRate.ToString();
        }
        public FrmAddInvoiceItem(FrmInvoiceItem FrmInvoiceItem, InvoiceItemService service)
        {
            InitializeComponent();
            this.FrmInvoiceItem = FrmInvoiceItem;
            this.service = service;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string Number = txtNumber.Text;
            decimal PricePerUnit = decimal.Parse(txtPricePerUnit.Text);
            int Quantity = int.Parse(txtQuantity.Text);
            decimal TotalPrice = decimal.Parse(txtTotalPrice.Text);
            decimal VAT = decimal.Parse(txtVAT.Text);
            decimal VATRate = decimal.Parse(txtVATRate.Text);
            if (isEditMode && CurrentInvoiceItem != null)
            {
                CurrentInvoiceItem.Number = Number;
                CurrentInvoiceItem.PricePerUnit = PricePerUnit;
                CurrentInvoiceItem.Quantity = Quantity;
                CurrentInvoiceItem.TotalPrice = TotalPrice;
                CurrentInvoiceItem.VAT = VAT;
                CurrentInvoiceItem.VATRate = VATRate;
                service.Update(CurrentInvoiceItem);
            }
            else
            {
                InvoiceItem newInvoiceItem = new InvoiceItem
                {
                    Id = Guid.NewGuid(),
                    Number= Number,
                    PricePerUnit = PricePerUnit,
                    Quantity = Quantity,
                    TotalPrice = TotalPrice,
                    VAT = VAT,
                    VATRate = VATRate
                };
                service.Create(newInvoiceItem);
            }
            this.Hide();
            FrmInvoiceItem.LoadInvoiceItems();
        }
        private void FrmAddInvoiceItem_KeyPress(object sender, KeyPressEventArgs e)
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