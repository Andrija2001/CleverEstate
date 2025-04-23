using CleverEstate.Forms.Clients;
using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Windows.Forms;
namespace CleverEstate.Forms.CatalogItem
{
    public partial class FrmAddItemCatalog : Form
    {
        ItemCatalogService service;
        FrmItemCatalog FrmItemCatalog;
        private ItemCatalog currentCatalogItem;
        private bool isEditMode;
        public FrmAddItemCatalog(FrmItemCatalog parentForm, ItemCatalogService service, ItemCatalog CatalogItemToEdit)
        : this(parentForm, service)
        {
            this.currentCatalogItem = CatalogItemToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditClients";
            button1.Text =  "OK";
            txtName.Text = CatalogItemToEdit.Name;
            txtPricePerUnit.Text = CatalogItemToEdit.PricePerUnit.ToString();
            txtUnit.Text = CatalogItemToEdit.Unit.ToString();
        }
        public FrmAddItemCatalog(FrmItemCatalog frmItemCatalog, ItemCatalogService service)
        {
            InitializeComponent();
            this.FrmItemCatalog = frmItemCatalog;
            this.service = service;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                ItemCatalog itemCatalog = new ItemCatalog();
                if (txtUnit.Text == "" || txtName.Text == "" || txtPricePerUnit.Text == "")
                {
                    return;
                }
                string Name = txtName.Text;
                int Unit = int.Parse(txtUnit.Text);
                decimal PricePerUnit = decimal.Parse(txtPricePerUnit.Text);
                itemCatalog.Id = Guid.NewGuid();
                itemCatalog.Name = Name;
                itemCatalog.Unit = Unit;
                itemCatalog.PricePerUnit = PricePerUnit;
                service.Create(itemCatalog);
                FrmItemCatalog.bindingSource1.Add(itemCatalog);
                FrmItemCatalog.PopulateDataGridView();
            }
            else
            {
                currentCatalogItem.Name = txtName.Text;
                currentCatalogItem.Unit = int.Parse(txtUnit.Text);
                currentCatalogItem.PricePerUnit = decimal.Parse(txtPricePerUnit.Text);
                service.Update(currentCatalogItem);
            }
            this.Close();
            
        }
        private void txtUnit_KeyPress(object sender, KeyPressEventArgs e)
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