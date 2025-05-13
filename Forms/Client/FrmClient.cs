using CleverEstate.Forms.Apartments;
using CleverEstate.Forms.Buildings;
using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface.Repository;
using CleverState.Services.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate.Forms.Clients
{
    public partial class FrmClient : Form
    {

        public BindingSource bindingSource1 = new BindingSource();
        private ClientService service;
        private Client selectedClient;
        private ClientRepository repository;
        private Button addNewRowButton = new Button();
        private Panel buttonPanel = new Panel();
        Font font = new Font("Arial", 12);
        public FrmClient()
        {
            InitializeComponent();
            InitializeDataGridView();
            SetupLayout();
            repository = new ClientRepository(new DataDbContext());
            bindingSource1.DataSource = typeof(Client);
        }
        private void InitializeDataGridView()
        {
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.AutoSizeRowsMode =
                 DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
        }

        private void FrmClient_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
        }

        public void LoadData()
        {
            var clientList = repository.GetAll();
            bindingSource1.DataSource = clientList;
            dataGridView1.DataSource = bindingSource1;
        }
        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);
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
            //FrmAddClient frmAddClient = new FrmAddClient(this, service);
            //  frmAddClient.ShowDialog();
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
                dataGridView1.Columns["Edit"].DisplayIndex = 7;
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
                dataGridView1.Columns["Delete"].DisplayIndex = 8;
            }
            if (dataGridView1.Columns.Contains("client_id"))
            {
                dataGridView1.Columns["client_id"].Visible = false;
                dataGridView1.Columns["apartment_id"].Visible = false;
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

                if (e.RowIndex >= 0)
                {
                    var selectedRow = dataGridView1.Rows[e.RowIndex];

                    selectedClient.Id = Guid.Parse(selectedRow.Cells["client_id"].Value.ToString());
                    selectedClient.Name = selectedRow.Cells["Name"].Value.ToString();
                    selectedClient.Surname = selectedRow.Cells["Surname"].Value.ToString();
                    selectedClient.PIB = int.Parse(selectedRow.Cells["PIB"].Value.ToString());
                    selectedClient.BankAccount = selectedRow.Cells["BankAccount"].Value.ToString();

               
                }
            }
        }
    }
}