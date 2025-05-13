using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverState.Services.Interface;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CleverEstate.Forms.Buildings
{
    public partial class FrmAddBuildings : Form
    {
        private readonly BuildingRepository buildingRepository;
        private readonly FrmBuildings parentForm;
        private readonly Building currentBuilding;
        private readonly bool isEditMode;
        public FrmAddBuildings(FrmBuildings parentForm, BuildingRepository buildingRepository)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.buildingRepository = buildingRepository; 
            this.currentBuilding = new Building();
            this.isEditMode = false;
        }
        public FrmAddBuildings(FrmBuildings parentForm, BuildingRepository buildingRepository, Building buildingToEdit)
            : this(parentForm, buildingRepository)
        {
            this.currentBuilding = buildingToEdit;
            this.isEditMode = true;
            this.Text = "Izmeni zgradu";
            button1.Text = "Sačuvaj izmene";
            txtAddress.Text = buildingToEdit.Address;
            textBox1.Text = buildingToEdit.City;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string address = txtAddress.Text.Trim();
            string city = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(city))
            {
                MessageBox.Show("Unesite i adresu i grad.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }  
            if (isEditMode)
            {
                currentBuilding.Address = address;
                currentBuilding.City = city;
                buildingRepository.Update(currentBuilding);
            }
            else
            {
                var newBuilding = new Building
                {
                    Id = Guid.NewGuid(),
                    Address = address,
                    City = city,
                };
                buildingRepository.Insert(newBuilding);
                parentForm.bindingSource1.Add(newBuilding);
            }
            parentForm.bindingSource1.ResetBindings(false);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
                return;

            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        public bool IsValidAddress(string address)
        {
            return Regex.IsMatch(address, @"^[a-zA-ZšđčćžŠĐČĆŽ\s]+$");
        }
    }
}
