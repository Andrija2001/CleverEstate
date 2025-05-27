using CleverEstate.Forms.Apartments;
using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.CatalogItem;
using CleverEstate.Forms.Invoices;
using CleverEstate.Forms.Invoices;
using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface.Repository;
using CleverState.Services.Classes;
using CleverState.Services.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate
{

    public partial class Form1 : Form
    {
        private InvoiceRepository repozitory;
        private InvoiceItemRepository invoiceItemRepository;
        private BuildingRepository buildingRepository;
        public BindingSource bindingSource1 = new BindingSource();
        private Font font = new Font("Segoe UI", 12);
        private Guid invoiceId; // polje u klasi

        public Form1()
        {
            InitializeComponent();
            repozitory = new InvoiceRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            invoiceItemRepository = new InvoiceItemRepository(new DataDbContext());
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 12);
            LoadComboBox();
         }

        private void LoadComboBox()
        {
            var buildings = buildingRepository.GetAll();
            comboBox1.DataSource = buildings;
            comboBox1.DisplayMember = "Address";
            comboBox1.ValueMember = "Id";
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void BuildingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBuildings frmBuildings = new FrmBuildings();
            frmBuildings.ShowDialog();
        }
        private void ApartmantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmApartment frmApartment = new FrmApartment();
            frmApartment.ShowDialog();
        }

        private void InvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                var invoiceForm = new FrmInvoice(this,repozitory);
                invoiceForm.LoadNewInvoiceItems();
                invoiceForm.ShowDialog();

            }
        }
        private void ItemCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmItemCatalog frmItemCatalog = new FrmItemCatalog();
            frmItemCatalog.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData(); 
        }
        public void LoadData()
        {
            var invoicelist = repozitory.GetAll();
            bindingSource1.DataSource = invoicelist;
            dataGridView1.DataSource = bindingSource1;
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (!dataGridView1.Columns.Contains("Open"))
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Open",
                    Text = "Open",
                    UseColumnTextForButtonValue = true,
                };
                dataGridView1.Columns.Add(btnDelete);
            }
            dataGridView1.Columns["Open"].DisplayIndex = dataGridView1.Columns.Count - 1;
            if (dataGridView1.Columns.Contains("Id"))
                dataGridView1.Columns["Id"].Visible = false;
            if (dataGridView1.Columns.Contains("ClientId"))
                dataGridView1.Columns["ClientId"].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Open" && e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                DateTime date = Convert.ToDateTime(row.Cells["Date"].Value);
                string month = row.Cells["Month"].Value.ToString();
                DateTime paymentDeadline = Convert.ToDateTime(row.Cells["PaymentDeadline"].Value);
                string period = row.Cells["Period"].Value.ToString();
                string invoiceNumber = row.Cells["InvoiceNumber"].Value.ToString();
                DateTime invoiceDate = Convert.ToDateTime(row.Cells["InvoiceDate"].Value);
                string description = row.Cells["Description"].Value.ToString();
                var selectedInvoice = (Invoice)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                Guid invoiceId = selectedInvoice.Id; FrmInvoice frmInvoice = new FrmInvoice(this,invoiceId);
                frmInvoice.SetInvoiceData(date, month, paymentDeadline, period, invoiceNumber, invoiceDate, description);
                frmInvoice.ShowDialog();
            }
        }
      
    }
}
