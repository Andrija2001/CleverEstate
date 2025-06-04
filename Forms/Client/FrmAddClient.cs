using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CleverEstate.Forms.Clients
{
    public partial class FrmAddClient : Form
    {

        private bool isEditMode;
        private ClientRepository _repository;
        private BuildingRepository buildingRepository;
        private ApartmentRepository apartmentRepository;
        private FrmClient parentForm;
        private Client currentClient;
        public FrmAddClient(FrmClient parentForm, ClientRepository repository)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            _repository = new ClientRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            apartmentRepository = new ApartmentRepository(new DataDbContext());

        }
        public FrmAddClient(FrmClient parentForm, ClientRepository repository, Client clientToEdit)
        : this(parentForm, repository)
        {
            this.currentClient = clientToEdit;
            this.isEditMode = true;
            this.Text = "Edit Client";
            button1.Text = "OK";
            txtName.Text = clientToEdit.Name;
            txtSurname.Text = clientToEdit.Surname;
            txtPIB.Text = clientToEdit.PIB.ToString();
            txtBankAccount.Text = clientToEdit.BankAccount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string surname = txtSurname.Text;
            string selectedAddress = comboBox1.Text;
            decimal area = decimal.Parse(textBox1.Text);

            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(selectedAddress))
            {
                MessageBox.Show("Molim vas popuniti sva polja.");
                return;
            }
            if (isEditMode)
            {
                currentClient.Name = name;
                currentClient.Surname = surname;
                currentClient.Address = selectedAddress;
                currentClient.PIB = string.IsNullOrEmpty(txtPIB.Text) ? 0 : int.Parse(txtPIB.Text);
                currentClient.BankAccount = string.IsNullOrEmpty(txtBankAccount.Text) ? "" : txtBankAccount.Text;
                _repository.Update(currentClient);
            }
            else
            {
                var newClient = new Client
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Surname = surname,
                    Address = selectedAddress,
                    PIB = string.IsNullOrEmpty(txtPIB.Text) ? 0 : int.Parse(txtPIB.Text),
                    BankAccount = string.IsNullOrEmpty(txtBankAccount.Text) ? "" : txtBankAccount.Text
                };
                _repository.Insert(newClient);
                parentForm.bindingSource1.Add(newClient);
            }
            parentForm.bindingSource1.ResetBindings(false);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
