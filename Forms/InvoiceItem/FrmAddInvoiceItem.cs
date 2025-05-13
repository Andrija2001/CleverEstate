using CleverEstate.Models;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface.Repository;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using CleverEstate.Services.Classes;
using CleverEstate.Forms.Invoices;
using static System.Windows.Forms.MonthCalendar;
using CleverEstate.Forms.Apartments;
using System.Linq;

namespace CleverEstate.Forms.InvoiceItems
{
    public partial class FrmAddInvoiceItem : Form
    {
        private FrmInvoice parentForm;
        private InvoiceItemRepository _invoiceItemRepository;
        private ItemCatalogRepository _itemCatalogRepository;
        private ClientRepository _clientRepository;
        private Guid _buildingId;
        private Guid _clientId;
        private Guid _invoiceId;



        public FrmAddInvoiceItem(FrmInvoice parent, InvoiceItemRepository invoiceItemRepository,
        ItemCatalogRepository itemCatalogRepository, ClientRepository clientRepository,
        Guid buildingId, Guid clientId, Guid invoiceId)
        {
            parentForm = parent;
            _invoiceItemRepository = invoiceItemRepository;
            _itemCatalogRepository = itemCatalogRepository;
            _clientRepository = clientRepository;
            _buildingId = buildingId;
            _clientId = clientId;
            _invoiceId = invoiceId;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            var itemCatalogId = (Guid)cmbItems.SelectedValue;
            var pricePerUnit = decimal.Parse(txtPricePerUnit.Text);
            var quantity = int.Parse(txtQuantity.Text);
            var vatRate = decimal.Parse(txtVATRate.Text);
            var vat = decimal.Parse(txtVAT.Text);
            var totalPrice = decimal.Parse(txtTotalPrice.Text);
            var number = txtNumber.Text;

            // Proveravamo da li već postoji stavka sa istim InvoiceId i ItemCatalogId
            var existingItem = _invoiceItemRepository.GetAll()
                .FirstOrDefault(item => item.InvoiceId == _invoiceId && item.ItemCatalogId == itemCatalogId);

            if (existingItem != null)
            {
                // Ako stavka već postoji, obavestite korisnika i nemojte dodavati novu stavku
                MessageBox.Show("Ova stavka već postoji za ovu fakturu.");
                return;  // Prestanite sa izvršavanjem funkcije
            }
            else
            {
                // Ako stavka ne postoji, kreiramo novu stavku
                var invoiceItem = new InvoiceItem
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = _invoiceId,
                    ItemCatalogId = itemCatalogId,
                    Quantity = quantity,
                    PricePerUnit = pricePerUnit,
                    VAT = vat,
                    VATRate = vatRate,
                    TotalPrice = totalPrice,
                    Number = number,
                };

                // Dodajemo novu stavku u bazu
                _invoiceItemRepository.Insert(invoiceItem);
                this.Close();
                parentForm.Filtriraj();
                
            }

        }


        private void FrmAddInvoiceItem_Load(object sender, EventArgs e)
        {
            FillComboBox();
        }

        private void FillComboBox()
        {
            var itemCatalog = _itemCatalogRepository.GetAll();
            cmbItems.DataSource = itemCatalog;
            cmbItems.DisplayMember = "Name";
            cmbItems.ValueMember = "Id";
            if (cmbItems.Items.Count > 0)
            {
                cmbItems.SelectedIndex = 0;
            }
        }
    }
}

