using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace CleverEstate.Forms.Apartments
{
    public partial class FrmApartment : Form
    {
        private Button addNewRowButton = new Button();
        private Panel buttonPanel = new Panel();
        private ApartmentService service;
        public BindingSource bindingSource1 = new BindingSource();
        Font font = new Font("Arial", 12);
        public FrmApartment()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            try
            {
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = bindingSource1;
                dataGridView1.AutoSizeRowsMode =
                     DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            }
            catch (SqlException)
            {
                MessageBox.Show("To run this sample replace connection.ConnectionString" +
                    " with a valid connection string to a Northwind" +
                    " database accessible to your system.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Threading.Thread.CurrentThread.Abort();
            }
        }
        private void FrmApartment_Load(object sender, EventArgs e)
        {
            service = new ApartmentService();
            SetupLayout();
            SetupDataGridView();
            PopulateDataGridView();
        }
        public void PopulateDataGridView()
        {
            var listaApartmana = service.GetAllApartments();
            bindingSource1.Clear();
            foreach (var apartment in listaApartmana)
            {
                var apartmentCopy = new Apartment
                {
                    Id = apartment.Id,
                    Area = apartment.Area,
                    Number = apartment.Number
                };
                bindingSource1.Add(apartmentCopy);
            }
        }
        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Name = "songsDataGridView";
            dataGridView1.Location = new Point(8, 8);
            dataGridView1.Size = new Size(500, 250);
            dataGridView1.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.Dock = DockStyle.Fill;
        }
        private void SetupLayout()
        {
            this.Size = new Size(600, 500);
            addNewRowButton.Text = "Add Row";
            addNewRowButton.Font = font;
            addNewRowButton.Location = new Point(10, 10);
            addNewRowButton.Click += new EventHandler(addNewRowButton_Click);
            buttonPanel.Controls.Add(addNewRowButton);
            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Bottom;
            this.Controls.Add(this.buttonPanel);
        }
        private void addNewRowButton_Click(object sender, EventArgs e)
        {
            FrmAddApartment frmAddApartment = new FrmAddApartment(this,service);
            frmAddApartment.ShowDialog();
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!dataGridView1.Columns.Contains("Edit"))
            {
                var btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                };

                dataGridView1.Columns.Add(btnEdit);
            }
            dataGridView1.Columns["Edit"].HeaderText = "";
            if (dataGridView1.Columns.Count > 2)
            {
                dataGridView1.Columns["Edit"].DisplayIndex = 5;
            }
            if (!dataGridView1.Columns.Contains("Delete"))
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                };
                dataGridView1.Columns.Add(btnDelete);
            }
            dataGridView1.Columns["Delete"].HeaderText = "";
            if (dataGridView1.Columns.Count > 3)
            {
                dataGridView1.Columns["Delete"].DisplayIndex = 4;
            }
            if (dataGridView1.Columns.Contains("ClientId") && dataGridView1.Columns.Contains("Id") && dataGridView1.Columns.Contains("BuildingId"))
            {
                dataGridView1.Columns["ClientId"].Visible = false;
                dataGridView1.Columns["Id"].Visible = false;
                dataGridView1.Columns["BuildingId"].Visible = false;
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                var selectedApartment = (Apartment)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                FrmAddApartment frm3 = new FrmAddApartment(this, service, selectedApartment);
                frm3.ShowDialog();
                int index = bindingSource1.IndexOf(selectedApartment);
                if (index != -1)
                {
                    var updatedApartment = new Apartment
                    {
                        Id = selectedApartment.Id,
                        Area = selectedApartment.Area,
                        Number = selectedApartment.Number
                    };
                    bindingSource1[index] = updatedApartment;
                    bindingSource1.ResetBindings(false); 
                }
            }
        }
    }
}