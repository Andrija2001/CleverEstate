using CleverEstate.Models;
using CleverEstate.Services.Classes.Repository;
using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;

namespace CleverEstate.Forms.Clients
{
    public partial class FrmClient : Form
    {
        public BindingSource bindingSource1 = new BindingSource();
        private ClientRepository repository;
        private Button addNewRowButton = new Button();
        private Panel buttonPanel = new Panel();
        Font font = new Font("Times New Roman", 14);
        public FrmClient()
        {
            InitializeComponent();
            repository = new ClientRepository(new DataDbContext());
            bindingSource1.DataSource = typeof(Client);
            dataGridView1.Font = font;
        }
        private void FrmClient_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        public void LoadData()
        {
            var clientList = repository.GetAll();
            bindingSource1.DataSource = clientList;
            dataGridView1.DataSource = bindingSource1;
        }
  
 
        private void addNewRowButton_Click(object sender, EventArgs e)
        {
            FrmAddClient frmAddClient = new FrmAddClient(this, repository);
            frmAddClient.ShowDialog();
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (!dataGridView1.Columns.Contains("Edit"))
            {
                var btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    Text = "Izmeni",
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
                    Text = "Obriši",
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
            if (dataGridView1.Columns.Contains("Id"))
            {
                dataGridView1.Columns["Id"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("InvoiceId"))
            {
                dataGridView1.Columns["InvoiceId"].Visible = false;
            }
            if (dataGridView1.Columns.Count > 3)
            {
                dataGridView1.Columns["Name"].HeaderText = "Ime";
                dataGridView1.Columns["Surname"].HeaderText = "Izmeni";
                dataGridView1.Columns["BankAccount"].HeaderText = "Tekući račun";
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                var clientToDelete = (Guid)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                repository.Delete(clientToDelete);
                BindingSource bindingSource = (BindingSource)dataGridView1.DataSource;
                bindingSource.RemoveAt(e.RowIndex);
                dataGridView1.Refresh();
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            { 
                if (e.RowIndex >= 0)
                {
                    var selectedClient = (Client)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                    FrmAddClient frmEdit = new FrmAddClient(this, repository, selectedClient);
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                    }
                }
            }
        }
    }
}
