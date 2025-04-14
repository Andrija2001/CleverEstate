using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate.Forms.Clients
{
    public partial class FrmClient : Form
    {
        private ClientService service;
        private List<Client> _dataSource;
        public List<Client> DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
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
            if (_dataSource == null)
                return;
            var exsistingClients = new HashSet<string>();
            foreach (var client in _dataSource)
            {
                string Key = client.Name + "-" + client.Surname;
                if (!exsistingClients.Contains(Key))
                {
                    AddRowToPanel(tableLayoutPanel1, client);
                    exsistingClients.Add(Key);
                }
            }
        }
        public FrmClient()
        {
            InitializeComponent();
            service = new ClientService();
            LoadClients();
        }
        public void LoadClients()
        {
            DataSource = service.GetAllClients();
        }
        private void AddRowToPanel(TableLayoutPanel panel, Client client)
        {
            int rowIndex = panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            Label lblName = new Label();
            lblName.DataBindings.Add("Text", client, "Name");
            panel.Controls.Add(lblName, 0, rowIndex);
            Label lblSurname = new Label();
            lblSurname.DataBindings.Add("Text", client, "Surname");
            panel.Controls.Add(lblSurname, 1, rowIndex);
            Label lblAddress = new Label();
            lblAddress.DataBindings.Add("Text", client, "Address");
            panel.Controls.Add(lblAddress, 2, rowIndex);
            Label lblCity = new Label();
            lblCity.DataBindings.Add("Text", client, "City");
            panel.Controls.Add(lblCity, 3, rowIndex);
            Label lblPIB = new Label();
            lblPIB.DataBindings.Add("Text", client, "PIB");
            panel.Controls.Add(lblPIB, 4, rowIndex);
            Label lblBankAccount = new Label();
            lblBankAccount.DataBindings.Add("Text", client, "BankAccount");
            panel.Controls.Add(lblBankAccount, 5, rowIndex);
            Button btnDelete = new Button() { Text = "Delete" };
            btnDelete.Click += (s, e) => {
                DeleteClient(client.Id);
            };
            panel.Controls.Add(btnDelete, 6, rowIndex);
            Button btnEdit = new Button() { Text = "Edit" };
            btnEdit.Click += (s, e) => {
                EditRow(client.Id);
            };
            panel.Controls.Add(btnEdit, 7, rowIndex);
        }
        private void DeleteClient(Guid id)
        {
            tableLayoutPanel1.SuspendLayout();
            service.Delete(id);
            LoadClients();
            tableLayoutPanel1.ResumeLayout();
        }
        private void EditRow(Guid ClientId)
        {
            var building = service.GetAllClients().FirstOrDefault(x => x.Id == ClientId);
            if (building != null)
            {
                FrmAddClient frmedit = new FrmAddClient(this, service, building);
                frmedit.ShowDialog();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAddClient frmAddClient= new FrmAddClient(this,service);
            frmAddClient.ShowDialog();
        }
    }
}