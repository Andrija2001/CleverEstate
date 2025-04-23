using CleverEstate.Forms.Apartments;
using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

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
            button1.Text = "OK";
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
            if (!isEditMode)
            {
                Building building = new Building();
                if (txtAddress.Text == "")
                {
                    return;
                }

                string address = txtAddress.Text;
                building.Id = Guid.NewGuid();
                building.Address = address;
                service.Create(building);
                FrmBuildings.bindingSource1.Add(building);
                FrmBuildings.PopulateDataGridView();
            }
            else
            {
                currentBuilding.Address = txtAddress.Text;
                service.Update(currentBuilding);
                
            }
            this.Close();
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