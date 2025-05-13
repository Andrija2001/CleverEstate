using CleverEstate.Forms.Clients;
using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface;
using CleverState.Services.Classes;
using System;
using System.Windows.Forms;
namespace CleverEstate.Forms.CatalogItem
{
    public partial class FrmAddItemCatalog : Form
    {
        private ItemCatalogRepository _repository;
        FrmItemCatalog FrmItemCatalog;
        private ItemCatalog currentCatalogItem;
        private bool isEditMode;
        public FrmAddItemCatalog(FrmItemCatalog parentForm, ItemCatalogRepository itemCatalogRepository, ItemCatalog CatalogItemToEdit)
        : this(parentForm, itemCatalogRepository)
        {
            this.currentCatalogItem = CatalogItemToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditClients";
            button1.Text =  "OK";
            txtName.Text = CatalogItemToEdit.Name;
            txtPricePerUnit.Text = CatalogItemToEdit.PricePerUnit.ToString();
            txtUnit.Text = CatalogItemToEdit.Unit.ToString();
        }
        public FrmAddItemCatalog(FrmItemCatalog frmItemCatalog, ItemCatalogRepository itemCatalogRepository)
        {
            InitializeComponent();
            this.FrmItemCatalog = frmItemCatalog;
            this._repository = itemCatalogRepository;
            _repository = new ItemCatalogRepository(new DataDbContext());

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
                _repository.Insert(itemCatalog);
                FrmItemCatalog.bindingSource1.Add(itemCatalog);
            }
            else
            {
                currentCatalogItem.Name = txtName.Text;
                currentCatalogItem.Unit = int.Parse(txtUnit.Text);
                currentCatalogItem.PricePerUnit = decimal.Parse(txtPricePerUnit.Text);
                _repository.Update(currentCatalogItem);
                this.Close();
            }
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
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
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