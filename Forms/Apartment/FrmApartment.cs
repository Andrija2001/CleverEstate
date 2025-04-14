using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate.Forms.Apartments
{

    public partial class FrmApartment : Form
    {
        private ApartmentService service;
        private List<Apartment> _dataSource;
        public List<Apartment> DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                DataBind(); 
            }
        }
        private void DataBind()
        {
            for (int i = tableLayoutPanel1.RowCount - 1; i > 0; i--)
            {
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(j, i);
                    if (control != null)
                        tableLayoutPanel1.Controls.Remove(control);
                }
            }
            while (tableLayoutPanel1.RowStyles.Count > 1)
            {
                tableLayoutPanel1.RowStyles.RemoveAt(1);
            }
            tableLayoutPanel1.RowCount = 1;
            if (_dataSource == null)
                return;
            var existingApartments = new HashSet<string>();
            foreach (var apartment in _dataSource)
            {
                string Key = apartment.Number.ToString() + "-" + apartment.Area.ToString();
                if (!existingApartments.Contains(Key))
                {
                    AddRowToPanel(tableLayoutPanel1, apartment);
                    existingApartments.Add(Key);
                }
            }
        }
        public FrmApartment()
        {
            InitializeComponent();
            service = new ApartmentService();
            LoadApartmants();
        }
        public void LoadApartmants()
        {
            DataSource = service.GetAllApartments();
        }
        private void AddRowToPanel(TableLayoutPanel panel, Apartment apartment)
        {
            int rowIndex = panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            Label lblNumber = new Label();
            lblNumber.DataBindings.Add("Text", apartment, "Number");
            panel.Controls.Add(lblNumber, 0, rowIndex);
            Label lblArea = new Label();
            lblArea.DataBindings.Add("Text", apartment, "Area");
            panel.Controls.Add(lblArea, 1, rowIndex);
            Button btnDelete = new Button() { Text = "Delete" };
            btnDelete.Click += (s, e) => {
                DeleteApartment(apartment.Id);
            };
            panel.Controls.Add(btnDelete, 2, rowIndex);
            Button btnEdit = new Button() { Text = "Edit" };
            btnEdit.Click += (s, e) => {
                EditRow(apartment.Id);
            };
            panel.Controls.Add(btnEdit, 3, rowIndex);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAddApartment frmAddApartment = new FrmAddApartment(this,service);
            frmAddApartment.ShowDialog();
        }
        private void EditRow(Guid Id)
        {
            var apartment = service.GetAllApartments().FirstOrDefault(x => x.Id == Id);
            if (apartment != null)
            {
                FrmAddApartment frmedit = new FrmAddApartment(this, service, apartment);
                frmedit.ShowDialog();
            }
        }
        private void DeleteApartment(Guid apartmentId)
        {  
          tableLayoutPanel1.SuspendLayout();
          service.Delete(apartmentId);
          LoadApartmants();
          tableLayoutPanel1.ResumeLayout();
        }
    } 
 }