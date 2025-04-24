using CleverEstate.Forms.CatalogItem;
using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Windows.Forms;
using System.Xml.Linq;
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
            button1.Text = "OK";
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
            if (!isEditMode)
            {
                InvoiceItem InvoiceItem = new InvoiceItem();
                if (txtNumber.Text == "" || txtPricePerUnit.Text == "" || txtQuantity.Text == "" || txtTotalPrice.Text == "" || txtVAT.Text == "" || txtVATRate.Text == "")
                {
                    return;
                }
                string Number = txtNumber.Text;
                decimal PricePerUnit = decimal.Parse(txtPricePerUnit.Text);
                int Quantity = int.Parse(txtQuantity.Text);
                decimal TotalPrice = decimal.Parse(txtTotalPrice.Text);
                decimal VAT=decimal.Parse(txtVAT.Text);
                decimal VATRate = decimal.Parse(txtVATRate.Text);
                InvoiceItem.Id = Guid.NewGuid();
                InvoiceItem.InvoiceId = Guid.NewGuid();
                InvoiceItem.ItemCatalogId = Guid.NewGuid();
                InvoiceItem.Number = Number;
                InvoiceItem.PricePerUnit= PricePerUnit;
                InvoiceItem.Quantity = Quantity;
                InvoiceItem.TotalPrice = TotalPrice;
                InvoiceItem.VAT= VAT;
                InvoiceItem.VATRate= VATRate;
                service.Create(InvoiceItem);
                FrmInvoiceItem.bindingSource1.Add(InvoiceItem);
                FrmInvoiceItem.PopulateDataGridView();
            }
            else
            {
                CurrentInvoiceItem.Number = txtNumber.Text;
                CurrentInvoiceItem.PricePerUnit = decimal.Parse(txtPricePerUnit.Text);
                CurrentInvoiceItem.Quantity =int.Parse(txtQuantity.Text);
                CurrentInvoiceItem.TotalPrice = decimal.Parse(txtTotalPrice.Text);
                CurrentInvoiceItem.VAT = decimal.Parse(txtVAT.Text);
                CurrentInvoiceItem.VATRate = decimal.Parse(txtVATRate.Text);
                service.Update(CurrentInvoiceItem);
                this.Close();
            }
         
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