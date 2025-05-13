using CleverEstate.Models;
using CleverEstate.Services.Classes.Repository;
using CleverState.Services.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CleverEstate.Forms.CatalogItem
{
    public partial class FrmItemCatalog : Form
    {
    
        public BindingSource bindingSource1 = new BindingSource();
        private ItemCatalogService service;
        private ItemCatalogRepository repository;
        Font font = new Font("Arial", 12);
        public FrmItemCatalog()
        {
            repository = new ItemCatalogRepository(new DataDbContext());
            InitializeComponent();
            bindingSource1.DataSource = typeof(ItemCatalog);

        }


        private void FrmItemCatalog_Load(object sender, EventArgs e)
        {
            LoadCatalogItems();
        }

        private void LoadCatalogItems()
        {
            var listItemCatalogs = repository.GetAll();
            bindingSource1.DataSource = listItemCatalogs;
            dataGridView1.DataSource = bindingSource1;
        }

        private void addNewRowButton_Click(object sender, EventArgs e)
        {
            FrmAddItemCatalog frmAddItemCatalog = new FrmAddItemCatalog(this, repository);
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
                var buildingToDelete = (Guid)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                repository.Delete(buildingToDelete); 
                BindingSource bindingSource = (BindingSource)dataGridView1.DataSource;
                bindingSource.RemoveAt(e.RowIndex);
                dataGridView1.Refresh();
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
                {
                    var selectedCatalogItem = (ItemCatalog)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                    FrmAddItemCatalog frm3 = new FrmAddItemCatalog(this, repository, selectedCatalogItem);
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
                        repository.Update(updatedItemCatalog);
                        bindingSource1[index] = updatedItemCatalog;
                        bindingSource1.ResetBindings(false);
                    }
                }
            }
        }
    }