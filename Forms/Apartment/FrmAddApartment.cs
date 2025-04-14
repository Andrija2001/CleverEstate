using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Windows.Forms;
namespace CleverEstate.Forms.Apartments
{
    public partial class FrmAddApartment : Form
    {
        ApartmentService service;
        FrmApartment frmApartment;
        private Apartment currentApartment;
        private bool isEditMode;
        public FrmAddApartment(FrmApartment parentForm, ApartmentService service, Apartment ApartmentToEdit)
       : this(parentForm, service)
        {
            this.currentApartment = ApartmentToEdit;
            this.isEditMode = true;
            this.Text = "FrmEditApartmants";
            button1.Text = "Edit Apartmant";
            txtArea.Text = ApartmentToEdit.Area.ToString();
            txtNumber.Text = ApartmentToEdit.Number.ToString();
        }

        public FrmAddApartment(FrmApartment frmApartment, ApartmentService service)
        {
            InitializeComponent();
            this.frmApartment = frmApartment;
            this.service = service;
        }
        private void btnAddApartmans_Click(object sender, EventArgs e)
        {
            int number = int.Parse(txtNumber.Text);
            decimal area = decimal.Parse(txtArea.Text);
            if (isEditMode && currentApartment != null)
            {
                currentApartment.Number = number;
                currentApartment.Area = area;
                service.Update(currentApartment);
            }
            else
            {
                Apartment newApartment = new Apartment
                {
                    Id = Guid.NewGuid(),
                    Number = number,
                    Area = area
                };
                service.Create(newApartment);
            }
            this.Hide();
            frmApartment.LoadApartmants();
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

        private void txtArea_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}