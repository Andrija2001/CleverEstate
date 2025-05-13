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
        private Guid currentClientId = Guid.Empty;
        private Guid currentApartmentId = Guid.Empty;
       // public FrmAddClient(FrmClient parentForm, ClientService service, Client client, Apartment apartment)
       //: this(parentForm, service)
       // {
       //     this.currentClient = client;
       //     this.currentClientId = client.Id;
       //     this.currentApartmentId = apartment.Id;
       //     this.isEditMode = true;
       //     this.Text = "Izmena klijenta";
       //     button1.Text = "Sačuvaj izmene";
       //     txtName.Text = client.Name;
       //     txtSurname.Text = client.Surname;
       //     txtPIB.Text = client.PIB.ToString();
       //     txtBankAccount.Text = client.BankAccount;
       //     txtApartmentNumber.Text = apartment.Number.ToString();
       //     textBox1.Text = apartment.Area.ToString();
       //     var buildings = buildingRepository.GetAll();
       //     comboBox1.DataSource = buildings;
       //     comboBox1.DisplayMember = "Address";
       //     comboBox1.ValueMember = "Id";
       //     comboBox1.SelectedValue = apartment.BuildingId;
       // }
        //public FrmAddClient(FrmClient frmClient, ClientService service)
        //{
        //    InitializeComponent();
        //    this.FrmClient = frmClient;
        //    this.service = service;
        //    _repository = new ClientRepository(new DataDbContext());
        //    buildingRepository = new BuildingRepository(new DataDbContext());
        //    apartmentRepository = new ApartmentRepository(new DataDbContext());
        //    LoadBuildings();
        //}
        private void LoadBuildings()
        {
            try
            {
                using (var conn = new Npgsql.NpgsqlConnection("Server=127.0.0.1;User Id=postgres; Password=1234;Database=CleverEstateDb;"))
                {
                    conn.Open();
                    var cmd = new Npgsql.NpgsqlCommand("SELECT \"Id\", \"Address\"\r\nFROM dbo.\"Buildings\"", conn);
                    var reader = cmd.ExecuteReader();
                    var ClientDict = new Dictionary<Guid, string>();
                    while (reader.Read())
                    {
                        var Id = reader.GetGuid(0);
                        string Address = reader.GetString(1);
                        ClientDict.Add(Id, Address);
                    }
                    reader.Close();
                    comboBox1.DataSource = new BindingSource(ClientDict, null);
                    comboBox1.DisplayMember = "Value";
                    comboBox1.ValueMember = "Key";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska prilikom uitavanja" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string apartmentNumberText = txtApartmentNumber.Text;
                string name = txtName.Text;
                string surname = txtSurname.Text;
                string selectedAddress = comboBox1.Text;
                decimal area = decimal.Parse(textBox1.Text);

                if (string.IsNullOrEmpty(apartmentNumberText) || string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(selectedAddress))
                {
                    MessageBox.Show("Molim vas popuniti sva polja.");
                    return;
                }
                int apartmentNumber = int.Parse(apartmentNumberText);
                Guid buildingId = GetSelectedBuildingId();
                if (isEditMode)
                {
                    var existingClient = _repository.GetById(currentClientId);
                    var existingApartment = apartmentRepository.GetById(currentApartmentId);
                    existingClient.Name = name;
                    existingClient.Surname = surname;
                    existingClient.Address = selectedAddress;
                    existingClient.PIB = string.IsNullOrEmpty(txtPIB.Text) ? 0 : int.Parse(txtPIB.Text);
                    existingClient.BankAccount = string.IsNullOrEmpty(txtBankAccount.Text) ? "" : txtBankAccount.Text;
                    existingApartment.Number = apartmentNumber;
                    existingApartment.BuildingId = buildingId;
                    existingApartment.Area = area;
                    _repository.Update(existingClient);
                    apartmentRepository.Update(existingApartment);
                    _repository.Save();
                    apartmentRepository.Save();
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
                    _repository.Save();
                    var newApartment = new Apartment
                    {
                        Id = Guid.NewGuid(),
                        Number = apartmentNumber,
                        Area = area,
                        BuildingId = buildingId,
                        ClientId = newClient.Id
                    };
                    apartmentRepository.Insert(newApartment);
                    apartmentRepository.Save();
                }
              //  FrmClient.UcitajKlijente();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Doslo je do greske: {ex.Message}");
            }
        }

        private Guid GetSelectedBuildingId()
        {
            if (comboBox1.SelectedItem != null)
            {
                var selectedBuilding = (Building)comboBox1.SelectedItem;
                return selectedBuilding.Id;
            }
            return Guid.Empty;
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

        private void FrmAddClient_Load(object sender, EventArgs e)
        {
            var buildings = buildingRepository.GetAll();
            comboBox1.DataSource = buildings;
            comboBox1.DisplayMember = "Address";
            comboBox1.ValueMember = "Id";
        }
    }
}
