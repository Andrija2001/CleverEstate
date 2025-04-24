using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.Clients;
using CleverEstate.Forms.InvoiceItems;
using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleverEstate.Forms.CatalogItem
{
    public partial class FrmItemCatalog : Form
    {
        public BindingSource bindingSource1 = new BindingSource();
        private ItemCatalogService service;
        private Button addNewRowButton = new Button();
        private Panel buttonPanel = new Panel();
        Font font = new Font("Arial", 12);
        public FrmItemCatalog()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            try
            {
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = bindingSource1;
                dataGridView1.AutoSizeRowsMode =
                     DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dataGridView1.BorderStyle = BorderStyle.Fixed3D;

            }
            catch (SqlException)
            {
                MessageBox.Show("To run this sample replace connection.ConnectionString" +
                    " with a valid connection string to a Northwind" +
                    " database accessible to your system.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Threading.Thread.CurrentThread.Abort();
            }
        }

        private void FrmItemCatalog_Load(object sender, EventArgs e)
        {
            service = new ItemCatalogService();
            SetupLayout();
            SetupDataGridView();
            PopulateDataGridView();
        }

        public void PopulateDataGridView()
        {
            var CatalogItemList = service.GetAllCatalogItems();
            bindingSource1.Clear();
            foreach (var catalog in CatalogItemList)
            {
                var ItemCatalogCopy = new ItemCatalog
                {
                    Id = catalog.Id,
                    Name = catalog.Name,
                    PricePerUnit = catalog.PricePerUnit,
                    Unit = catalog.Unit

                };
                bindingSource1.Add(ItemCatalogCopy);

            }
        }
        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);
            dataGridView1.ColumnHeadersVisible = true;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Name = "songsDataGridView";
            dataGridView1.Location = new Point(8, 8);
            dataGridView1.Size = new Size(500, 250);
            dataGridView1.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
           dataGridView1.Dock = DockStyle.Fill;
        }

        private void SetupLayout()
        {
            this.Size = new Size(600, 500);
            addNewRowButton.Text = "Add Row";
            addNewRowButton.Font = font;
            addNewRowButton.Location = new Point(10, 10);
            addNewRowButton.Click += new EventHandler(addNewRowButton_Click);
            buttonPanel.Controls.Add(addNewRowButton);
            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Bottom;
            this.Controls.Add(this.buttonPanel);
        }
        private void addNewRowButton_Click(object sender, EventArgs e)
        {
            FrmAddItemCatalog frmAddItemCatalog = new FrmAddItemCatalog(this, service);
            frmAddItemCatalog.ShowDialog();

        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!dataGridView1.Columns.Contains("Edit"))
            {
                var btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                };

                dataGridView1.Columns.Add(btnEdit);
            }
            dataGridView1.Columns["Edit"].HeaderText = "";
            if (dataGridView1.Columns.Count > 2)
            {
                dataGridView1.Columns["Edit"].DisplayIndex = 4;
            }
            if (!dataGridView1.Columns.Contains("Delete"))
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                };
                dataGridView1.Columns.Add(btnDelete);
            }
            dataGridView1.Columns["Delete"].HeaderText = "";
            if (dataGridView1.Columns.Count > 3)
            {
                dataGridView1.Columns["Delete"].DisplayIndex = 5;
            }
            if (dataGridView1.Columns.Contains("Id"))
            {
                dataGridView1.Columns["Id"].Visible = false;
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                var selectedCatalogItem = (ItemCatalog)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                FrmAddItemCatalog frm3 = new FrmAddItemCatalog(this, service, selectedCatalogItem);
                frm3.ShowDialog();
                int index = bindingSource1.IndexOf(selectedCatalogItem);
                if (index != -1)
                {
                    var updatedItemCatalog = new ItemCatalog
                    {
                        Id = selectedCatalogItem.Id,
                        Name = selectedCatalogItem.Name,
                        PricePerUnit = selectedCatalogItem.PricePerUnit,
                        Unit = selectedCatalogItem.Unit,
                    };
                    bindingSource1[index] = updatedItemCatalog;
                    bindingSource1.ResetBindings(false);
                }
            }
        }
    }
}