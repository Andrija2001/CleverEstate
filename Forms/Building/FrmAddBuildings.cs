using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CleverEstate.Forms.Buildings
{
    public partial class FrmAddBuildings : Form
    {
        BuildingService service;
        FrmBuildings FrmBuildings;
        private Building currentBuilding;
        private bool isEditMode;
        public bool IsValidAddress(string address)
        {
            return Regex.IsMatch(address, @"^[a-zA-Z\s]+$");
        }
        public FrmAddBuildings(FrmBuildings parentForm, BuildingService service, Building buildingToEdit)
      : this(parentForm, service)
        {
            this.currentBuilding = buildingToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditBuilding";
            button1.Text = "Edit Building";
            txtAddress.Text = buildingToEdit.Address;
        }
        public FrmAddBuildings(FrmBuildings frmBuildings, BuildingService service)
        {
            InitializeComponent();
            this.FrmBuildings = frmBuildings;
            this.service = service;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string address = txtAddress.Text;
            if (isEditMode && currentBuilding != null)
            {
                currentBuilding.Address = address;
                service.Update(currentBuilding);
            }
            else
            {
                Building newBuilding = new Building
                {
                    Id = Guid.NewGuid(),
                    Address = address
                };
                service.Create(newBuilding);
            }
            this.Hide();
            FrmBuildings.LoadBuildings(); 
        }

        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
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