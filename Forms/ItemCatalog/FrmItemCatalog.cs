using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.Clients;
using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleverEstate.Forms.CatalogItem
{
    public partial class FrmItemCatalog : Form
    {
        private ItemCatalogService service;
        private List<ItemCatalog> datasource;
        public List<ItemCatalog> DataSource
        {
            get => datasource;
            set
            {
                datasource = value;
                DataBind();
            }
        }
        private void DataBind()
        {
            for (int i = tableLayoutPanel1.RowCount - 1; i > 0; i--)
            {
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(j, i);
                    if (control != null)
                        tableLayoutPanel1.Controls.Remove(control);
                }
            }
            while (tableLayoutPanel1.RowStyles.Count > 1)
            {
                tableLayoutPanel1.RowStyles.RemoveAt(1);
            }
            tableLayoutPanel1.RowCount = 1;
            if (datasource == null)
                return;
            var exsistingCatalogItem = new HashSet<string>();
            foreach (var catalogitem in datasource)
            {
                string Key = catalogitem.Name;
                if (!exsistingCatalogItem.Contains(Key))
                {
                    AddRowToPanel(tableLayoutPanel1, catalogitem);
                    exsistingCatalogItem.Add(Key);
                }
            }
        }
        public FrmItemCatalog()
        {
            InitializeComponent();
            service = new ItemCatalogService();
            LoadItemCatalog();
        }
        public void LoadItemCatalog()
        {
            DataSource = service.GetAllCatalogItems();
        }
        private void AddRowToPanel(TableLayoutPanel panel, ItemCatalog CatalogItem)
        {
            int rowIndex = panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            Label lblName = new Label();
            lblName.DataBindings.Add("Text", CatalogItem, "Name");
            panel.Controls.Add(lblName, 0, rowIndex);
            Label lblPricePerUnit = new Label();
            lblPricePerUnit.DataBindings.Add("Text", CatalogItem, "PricePerUnit");
            panel.Controls.Add(lblPricePerUnit, 1, rowIndex);
            Label lblUnit = new Label();
            lblUnit.DataBindings.Add("Text", CatalogItem, "Unit");
            panel.Controls.Add(lblUnit, 2, rowIndex);
            Button btnDelete = new Button() { Text = "Delete" };
            btnDelete.Click += (s, e) => {
                DeleteBuilding(CatalogItem.Id);
            };
            panel.Controls.Add(btnDelete, 3, rowIndex);
            Button btnEdit = new Button() { Text = "Edit" };
            btnEdit.Click += (s, e) => {
                EditRow(CatalogItem.Id);
            };
            panel.Controls.Add(btnEdit, 4, rowIndex);
        }
        private void EditRow(Guid id)
        {
            var building = service.GetAllCatalogItems().FirstOrDefault(x => x.Id == id);
            if (building != null)
            {
                FrmAddItemCatalog frmedit = new FrmAddItemCatalog(this, service, building);
                frmedit.ShowDialog();
            }
        }
        private void DeleteBuilding(Guid Id)
        {
            tableLayoutPanel1.SuspendLayout();
            service.Delete(Id);
            LoadItemCatalog();
            tableLayoutPanel1.ResumeLayout();
        }
        private void DeleteRow(TableLayoutPanel panel, int rowIndex)
        {
            var Namelbl = tableLayoutPanel1.GetControlFromPosition(0, rowIndex) as Label;
            var PricePerUnitlbl = tableLayoutPanel1.GetControlFromPosition(1, rowIndex) as Label;
            var Unitlbl = tableLayoutPanel1.GetControlFromPosition(2, rowIndex) as Label;
            if (Namelbl != null && PricePerUnitlbl != null && Unitlbl != null)
            {
                string Name = Namelbl.Text;
                decimal PricePerUnit = decimal.Parse(PricePerUnitlbl.Text);
                int Unit = int.Parse(Unitlbl.Text);
                var RemoveCatalogItem = service.GetAllCatalogItems().FirstOrDefault(a => a.Name == Name && a.PricePerUnit == PricePerUnit && a.Unit == Unit);
                if (RemoveCatalogItem != null)
                {
                    service.Delete(RemoveCatalogItem.Id);

                }
            }
            for (int i = 0; i < panel.ColumnCount; i++)
            {
                Control control = panel.GetControlFromPosition(i, rowIndex);
                if (control != null)
                {
                    panel.Controls.Remove(control);
                }
            }
            panel.RowCount--;
            panel.Refresh();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAddItemCatalog frmAddItemCatalog = new FrmAddItemCatalog(this, service);
            frmAddItemCatalog.ShowDialog();
        }
    }
}