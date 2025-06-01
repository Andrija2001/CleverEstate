using CleverEstate.Forms;
using CleverEstate.Forms.Apartments;
using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.CatalogItem;
using CleverEstate.Forms.Clients;
using CleverEstate.Forms.Invoices;
using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
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
        private ItemCatalogRepository itemCatalogRepository;
        private InvoiceItemRepository invoiceItemRepository;
        private BuildingRepository buildingRepository;
        private ClientRepository clientRepository;
        private InvoiceRepository invoiceRepository;
        private ApartmentRepository apartmentRepository;
        public BindingSource bindingSource1 = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            itemCatalogRepository = new ItemCatalogRepository(new DataDbContext());
            apartmentRepository = new ApartmentRepository(new DataDbContext());
            clientRepository = new ClientRepository(new DataDbContext());
            invoiceRepository = new InvoiceRepository(new DataDbContext());
            repozitory = new InvoiceRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            invoiceItemRepository = new InvoiceItemRepository(new DataDbContext());
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 12);
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
            var invoiceForm = new FrmInvoice(this, repozitory);
            invoiceForm.LoadNewInvoiceItems();
            invoiceForm.ShowDialog();
        }
        private void ItemCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmItemCatalog frmItemCatalog = new FrmItemCatalog();
            frmItemCatalog.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today.AddMonths(-1);
        }
       private void DateFilterChanged()
        {
            DateTime from = dateTimePicker1.Value.Date;
            DateTime to = dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1);
            var allData = repozitory.GetAll();
            var filtrirano = allData
                .Where(i => i.Date >= from && i.Date <= to)
                .ToList();
            bindingSource1.DataSource = filtrirano;
            dataGridView1.Refresh();
        }

        public void LoadData()
        {
            var invoicelist = repozitory.GetAll();
            bindingSource1.DataSource = invoicelist;
            dataGridView1.DataSource = bindingSource1;
            PrevediKoloneNaSrpski(dataGridView1);


        }
        private void PrevediKoloneNaSrpski(DataGridView dgv)
        {
            var prevodi = new Dictionary<string, string>()
                {
                    {"Date", "Datum"},
                    {"Month", "Mesec"},
                    {"PaymentDeadline", "Rok plaćanja"},
                    {"Period", "Period"},
                    {"InvoiceNumber", "Broj računa"},
                    {"InvoiceDate", "Datum računa"},
                    {"InvoiceStatus", "Status plaćanja"},
                };

            foreach (DataGridViewColumn kolona in dgv.Columns)
            {
                if (prevodi.ContainsKey(kolona.Name))
                {
                    kolona.HeaderText = prevodi[kolona.Name];
                }
            }
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (!dataGridView1.Columns.Contains("Open"))
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Open",
                    Text = "Otvori",
                    UseColumnTextForButtonValue = true,
                };
                dataGridView1.Columns.Add(btnDelete);
            }
            dataGridView1.Columns["Open"].DisplayIndex = dataGridView1.Columns.Count - 2;
            if (dataGridView1.Columns.Contains("Id"))
                dataGridView1.Columns["Id"].Visible = false;
            if (dataGridView1.Columns.Contains("ClientId"))
                dataGridView1.Columns["ClientId"].Visible = false;
            if (dataGridView1.Columns.Contains("Open"))
            {
                dataGridView1.Columns["Open"].HeaderText = "Otvori";
            }
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
                var selectedInvoice = (Invoice)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                Guid invoiceId = selectedInvoice.Id; FrmInvoice frmInvoice = new FrmInvoice(this, invoiceId);
                frmInvoice.SetInvoiceData(invoiceId, date, month, paymentDeadline, period, invoiceNumber, invoiceDate);
                frmInvoice.ShowDialog();
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateFilterChanged();
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateFilterChanged();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            var filtered = repozitory.GetAll()
                .Where(i => i.InvoiceNumber.ToString().StartsWith(searchText))
                .ToList();

            bindingSource1.DataSource = filtered;
            dataGridView1.Refresh();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selected))
                return;
            var allInvoices = repozitory.GetAll();
            List<Invoice> filteredInvoices;
            if (selected.Equals("Plaćeni", StringComparison.OrdinalIgnoreCase))
                filteredInvoices = allInvoices.Where(inv => inv.InvoiceStatus).ToList();
            else if (selected.Equals("Neplaćeni", StringComparison.OrdinalIgnoreCase))
                filteredInvoices = allInvoices.Where(inv => !inv.InvoiceStatus).ToList();
            else
                filteredInvoices = allInvoices.ToList();
            bindingSource1.DataSource = filteredInvoices;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.Refresh();
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var column = dataGridView1.Columns[e.ColumnIndex];
            if (column.Name == "InvoiceStatus")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var invoice = row.DataBoundItem as Invoice;
                if (invoice == null) return;
                var cellValue = row.Cells[e.ColumnIndex].Value;
                bool newStatus = false;
                if (cellValue != null && bool.TryParse(cellValue.ToString(), out bool val))
                {
                    newStatus = val;
                }
                invoice.InvoiceStatus = newStatus;
                try
                {
                    repozitory.Update(invoice);
                    repozitory.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška prilikom ažuriranja baze: {ex.Message}");
                }
            }
        }
        private void grupniRačunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmGrupniRacun frmGrupniRacun = new FrmGrupniRacun(this,buildingRepository, invoiceItemRepository, itemCatalogRepository, apartmentRepository, clientRepository, invoiceRepository);
            frmGrupniRacun.ShowDialog();
        }
        private void klijentiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClient frmClient = new FrmClient();
            frmClient.ShowDialog();
        }
    }
}
