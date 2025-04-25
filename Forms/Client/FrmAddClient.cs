using Castle.Components.DictionaryAdapter.Xml;
using CleverEstate.Forms.Buildings;
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
            button1.Text = "OK";
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
            if (!isEditMode)
            {
                Client client = new Client();
                if (txtAddress.Text == "" || txtName.Text == "" || txtSurname.Text == "" || txtCity.Text == "" || txtPIB.Text == "" || txtBankAccount.Text == "")
                {
                    return;
                }
                string Name = txtName.Text;
                string Surname = txtSurname.Text;
                string Address = txtAddress.Text;
                string City = txtCity.Text;
                int PIB = int.Parse(txtPIB.Text);
                string BankAccount = txtBankAccount.Text;  
                client.Id = Guid.NewGuid();
                client.InvoiceId = Guid.NewGuid();
                client.Name = Name;
                client.Surname = Surname;
                client.Address = Address;   
                client.City = City;
                client.BankAccount = BankAccount;
                client.PIB = PIB;
                service.Create(client);
                FrmClient.bindingSource1.Add(client);
                FrmClient.PopulateDataGridView();                
            }
            else
            {
                currentClient.Name = txtName.Text;
                currentClient.Surname = txtSurname.Text;
                currentClient.Address = txtAddress.Text;
                currentClient.City = txtCity.Text;
                currentClient.BankAccount = txtBankAccount.Text;
                currentClient.PIB = int.Parse(txtPIB.Text);
                service.Update(currentClient);
                this.Close();
            }
            this.Close();
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
