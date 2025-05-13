using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface;
using CleverEstate.Services.Interface.Repository;
using CleverState.Services.Classes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate.Forms.Apartments
{
    public partial class FrmAddApartment : Form
    {
        private ApartmentRepository apartmentRepository;
        private ClientRepository clientRepository;
        FrmApartment parentForm;
        private Client currentClient;
        private Apartment currentApartment;
        private readonly Guid buildingId;
        private bool isEditMode;
        public FrmAddApartment(FrmApartment parentForm, ApartmentRepository apartmentRepository, Apartment apartmentToEdit, Guid buildingId, ClientRepository clientRepository, Client currentClient)
            : this(parentForm, apartmentRepository, buildingId, clientRepository)
        {
            this.currentClient = currentClient;
            this.currentApartment = apartmentToEdit;
            this.isEditMode = true;
            this.Text = "Edit Apartment";
            button1.Text = "OK";
        }
        public FrmAddApartment(FrmApartment parentForm, ApartmentRepository apartmentRepository, Guid buildingId, ClientRepository clientRepository)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.apartmentRepository = apartmentRepository;
            this.currentApartment = new Apartment();
            this.isEditMode = false;
            this.buildingId = buildingId;
            this.clientRepository = clientRepository;
        }
        private void btnAddApartmans_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtArea.Text.Trim(), out decimal area))
            {
                MessageBox.Show("Unesite validnu površinu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!int.TryParse(txtNumber.Text.Trim(), out int number))
            {
                MessageBox.Show("Unesite validan broj apartmana.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cmbClients.SelectedValue == null || !(cmbClients.SelectedValue is Guid selectedClientId))
            {
                MessageBox.Show("Izaberite klijenta.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (isEditMode)
            {
                currentApartment.Area = area;
                currentApartment.Number = number;
                currentApartment.ClientId = selectedClientId;

                apartmentRepository.Update(currentApartment);
                apartmentRepository.Save();
                parentForm.LoadApartmants();
            }
            else
            {
                var newApartment = new Apartment
                {
                    Id = Guid.NewGuid(),
                    Area = area,
                    Number = number,
                    ClientId = selectedClientId,
                    BuildingId = buildingId, 
                };

                apartmentRepository.Insert(newApartment);
                apartmentRepository.Save();
                
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void FrmAddApartment_Load(object sender, EventArgs e)
        {
            LoadClients();
            if (isEditMode)
            {
                txtArea.Text = currentApartment.Area.ToString();
                txtNumber.Text = currentApartment.Number.ToString();
                cmbClients.SelectedValue = currentApartment.ClientId;
            }
        }
        private void LoadClients()
        {
            var clients = clientRepository.GetAll();
            var displayList = clients.Select(c => new
            {
                FullName = c.Name + " " + c.Surname,
                c.Id
            }).ToList();
            cmbClients.DataSource = displayList;
            cmbClients.DisplayMember = "FullName";
            cmbClients.ValueMember = "Id";
        }
    }
}