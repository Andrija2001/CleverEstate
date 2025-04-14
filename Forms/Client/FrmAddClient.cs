using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Windows.Forms;

namespace CleverEstate.Forms.Clients
{
    public partial class FrmAddClient : Form
    {
        ClientService service;
        FrmClient FrmClient;
        private Client currentClient;
        private bool isEditMode;
        public FrmAddClient(FrmClient parentForm, ClientService service, Client ClientToEdit)
       : this(parentForm, service)
        {
            this.currentClient = ClientToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditClients";
            button1.Text = "Edit Client";
            txtName.Text = ClientToEdit.Name;
            txtSurname.Text = ClientToEdit.Surname;
            txtAddress.Text = ClientToEdit.Address;
            txtCity.Text = ClientToEdit.City;
            txtPIB.Text = ClientToEdit.PIB.ToString();
            txtBankAccount.Text = ClientToEdit.BankAccount;
        }
        public FrmAddClient(FrmClient frmClient, ClientService service)
        {
            InitializeComponent();
            this.FrmClient = frmClient;
            this.service = service;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string Surname = txtSurname.Text;
            string Address = txtAddress.Text;   
            string City= txtCity.Text;
            int PIB = int.Parse(txtPIB.Text);
            string BankAccount = txtBankAccount.Text;
            if (isEditMode && currentClient != null)
            {
                currentClient.Name = Name;
                currentClient.Surname = Surname;
                currentClient.Address = Address;
                currentClient.City= City;
                currentClient.PIB = PIB;
                currentClient.BankAccount = BankAccount;
                service.Update(currentClient);
            }
            else
            {
                Client newClient = new Client
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Surname = Surname,
                    Address = Address,
                    City = City,
                    PIB = PIB,
                    BankAccount = BankAccount
                };
                service.Create(newClient);
            }
            this.Hide();
            FrmClient.LoadClients();
        }
        private void txtPIB_KeyPress(object sender, KeyPressEventArgs e)
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