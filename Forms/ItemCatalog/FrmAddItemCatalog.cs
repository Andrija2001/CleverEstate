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
            button1.Text = "Edit Client";
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
            string Name = txtName.Text;
            decimal PricePerUnit = decimal.Parse(txtPricePerUnit.Text);
            int Unit = int.Parse(txtUnit.Text);
            if (isEditMode && currentCatalogItem != null)
            {
                currentCatalogItem.Name = Name;
                currentCatalogItem.PricePerUnit = PricePerUnit;
                currentCatalogItem.Unit = Unit;
                service.Update(currentCatalogItem);
            }
            else
            {
                ItemCatalog newCatalogItem = new ItemCatalog
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    PricePerUnit = PricePerUnit,
                    Unit = Unit
                };
                service.Create(newCatalogItem);
            }
            this.Hide();
            FrmItemCatalog.LoadItemCatalog();
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