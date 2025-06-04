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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using CleverEstate.Forms.Invoices;

namespace CleverEstate.Forms.InvoiceItems
{
    public partial class FrmAddInvoiceItem : Form
    {
        private FrmInvoice parentForm;
        private InvoiceItemRepository _invoiceItemRepository;
        private ItemCatalogRepository _itemCatalogRepository;
        private ClientRepository _clientRepository;
        private InvoiceRepository invoiceRepository;
        private Guid _buildingId;
        private Guid _clientId;
        private Guid _invoiceId;
        public FrmAddInvoiceItem(FrmInvoice parent,InvoiceRepository repository, InvoiceItemRepository invoiceItemRepository,
        ItemCatalogRepository itemCatalogRepository, ClientRepository clientRepository,
        Guid buildingId, Guid clientId, Guid invoiceId)
        {
            parentForm = parent;
            _invoiceItemRepository = invoiceItemRepository;
            _itemCatalogRepository = itemCatalogRepository;
            _clientRepository = clientRepository;
            invoiceRepository = repository;
            _buildingId = buildingId;
            _clientId = clientId;
            _invoiceId = invoiceId;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPricePerUnit.Text, out var pricePerUnit))
            {
                MessageBox.Show("Unesite cenu po jedinici.");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out var quantity))
            {
                MessageBox.Show("Unesite količinu.");
                return;
            }
            var itemCatalogId = (Guid)cmbItems.SelectedValue;
            var VAT = StringToDecimal(cmbVAT.SelectedItem.ToString());
            var VATRate = cmbVATRate.Text;
            if (cmbVATRate.SelectedItem == null)
            {
                MessageBox.Show("Odaberite stopu PDV-a.");
                return;
            }
            var allInvoiceItems = _invoiceItemRepository.GetAll();
            var allInvoices = invoiceRepository.GetAll();

            foreach (DataGridViewRow row in parentForm.dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                var rowItemCatalogId = (Guid)row.Cells["ItemCatalogId"].Value;
                if (rowItemCatalogId == itemCatalogId)
                {
                    MessageBox.Show("Ova stavka već postoji u ovoj fakturi.");
                    return;
                }
            }

            var invoiceItem = new InvoiceItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = _invoiceId,
                ItemCatalogId = itemCatalogId,
                Quantity = quantity,
                PricePerUnit = pricePerUnit,
                VAT = VAT,
                VATRate = VATRate,
            };

            _invoiceItemRepository.Insert(invoiceItem);
            this.Close();
            parentForm.LoadInvoiceItems(_invoiceId);
        }
        private decimal StringToDecimal(string vatStr)
        {
            if (string.IsNullOrEmpty(vatStr))
                return 0m;

            vatStr = vatStr.Trim().Replace("%", ""); 

            if (decimal.TryParse(vatStr, out decimal vatValue))
            {
                return vatValue / 100m; 
            }
            else
            {
                return 0m; 
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

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadInvoiceItemDetails();
        }
        public void LoadInvoiceItemDetails()
        {
            if (cmbItems.SelectedValue is Guid selectedCatalogId)
            {
                var invoiceItem = _invoiceItemRepository.GetAll()
                    .FirstOrDefault(ii => ii.InvoiceId == _invoiceId && ii.ItemCatalogId == selectedCatalogId);
                if (invoiceItem != null)
                {
                    var catalogItem = _itemCatalogRepository.GetById(invoiceItem.ItemCatalogId);

                    txtQuantity.Text = invoiceItem.Quantity.ToString();
                    txtPricePerUnit.Text = invoiceItem.PricePerUnit.ToString();
                    cmbVAT.SelectedItem = invoiceItem.VAT;
                    cmbVATRate.SelectedItem = invoiceItem.VATRate;
                }
                else
                {
                    txtQuantity.Text = "";
                    txtPricePerUnit.Text = "";
                    cmbVAT.Text = "";
                    cmbVATRate.Text = "";
                }
            }
        }
        private void cmbVATRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = cmbVATRate.SelectedItem?.ToString();

            if (selectedValue == "Opšta stopa")
            {
                cmbVAT.SelectedItem = "20%";
            }
            else if (selectedValue == "Posebna stopa")
            {
                cmbVAT.SelectedItem = "10%";
            }
            else
            {
                cmbVAT.SelectedItem = null;
            }
        }

    }
}
