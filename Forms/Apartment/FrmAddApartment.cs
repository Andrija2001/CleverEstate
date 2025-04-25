using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Windows.Forms;
namespace CleverEstate.Forms.Apartments
{
   public partial class FrmAddApartment : Form
    {
        FrmApartment frmApartment;
        ApartmentService service;
        private Apartment currentApartment;
        private bool isEditMode;
        public FrmAddApartment(FrmApartment parentForm, ApartmentService service, Apartment ApartmentToEdit)
            : this(parentForm, service)
        {
            this.currentApartment = ApartmentToEdit;
            this.isEditMode = true;
            this.Text = "FrmEdit";
            button1.Text = "Ok";
            txtArea.Text = ApartmentToEdit.Area.ToString();
            txtNumber.Text = ApartmentToEdit.Number.ToString();
        }
        public FrmAddApartment(FrmApartment formApartment, ApartmentService service)
        {
            InitializeComponent();
            this.service = service;
            frmApartment = formApartment;
        }
        private void btnAddApartmans_Click(object sender, EventArgs e)
        {
            if (!isEditMode)
            {
                Apartment apartments = new Apartment();
                if (!int.TryParse(txtNumber.Text, out int number) || !decimal.TryParse(txtArea.Text, out decimal area))
                {
                    return;
                }
                 number = int.Parse(txtNumber.Text);
                 area = decimal.Parse(txtArea.Text);
                apartments.Id = Guid.NewGuid();
                apartments.Area = area;
                apartments.Number = number;
                service.Create(apartments);
                frmApartment.bindingSource1.Add(apartments);
                frmApartment.PopulateDataGridView();
            }
            else
            {
                currentApartment.Area = decimal.Parse(txtArea.Text);
                currentApartment.Number = int.Parse(txtNumber.Text);
                service.Update(currentApartment);
                this.Close();
            }
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
    }
}