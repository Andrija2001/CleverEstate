using CleverEstate.Forms.Apartments;
using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.Clients;
using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface;
using CleverEstate.Services.Interface.Repository;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CleverEstate.Forms.InvoiceItems
{
    public partial class FrmInvoiceItem : Form
    {

        public BindingSource bindingSource1 = new BindingSource();
        private Client selectedClient;
        private InvoiceItemRepository repository;
        private ApartmentRepository apartmentRepository;
        private ClientRepository clientRepository;
        private BuildingRepository buildingRepository;
        private ItemCatalogRepository itemCatalogRepository;
        private InvoiceRepository invoiceRepository;

        private Button addNewRowButton = new Button();
        private Panel buttonPanel = new Panel();
        Font font = new Font("Arial", 12);
        public FrmInvoiceItem()
        {
            InitializeComponent();
            InitializeDataGridView();
            itemCatalogRepository = new ItemCatalogRepository(new DataDbContext());
            repository = new InvoiceItemRepository(new DataDbContext());
            clientRepository = new ClientRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            invoiceRepository = new InvoiceRepository(new DataDbContext());
            apartmentRepository = new ApartmentRepository(new DataDbContext());
            bindingSource1.DataSource = typeof(Client);
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.AutoSizeRowsMode =
                 DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
        }

        private void FrmInvoiceItem_Load(object sender, EventArgs e)
        {
            FillComboBoxs();
            LoadInvoiceItems();
            ReorderColumns();

        }

        private void ReorderColumns()
        {
            dataGridView1.Columns["Number"].DisplayIndex = 0;  // Stavite PricePerUnit na prvu poziciju
            dataGridView1.Columns["ItemName"].DisplayIndex = 1;     // Stavite ItemName na drugu poziciju
            dataGridView1.Columns["PricePerUnit"].DisplayIndex = 2;
            dataGridView1.Columns["Area"].DisplayIndex = 3;
            dataGridView1.Columns["Unit"].DisplayIndex = 4;
            dataGridView1.Columns["Total"].DisplayIndex = 5;


        }

        public void LoadInvoiceItems()
        {
            var invoiceItems = repository.GetAll();
            var apartments = apartmentRepository.GetAll();
            var clients = clientRepository.GetAll();
            var itemCatalog = itemCatalogRepository.GetAll();
            var invoices = invoiceRepository.GetAll();

            if (!invoiceItems.Any())
                MessageBox.Show("Nema faktura!");
            if (!apartments.Any())
                MessageBox.Show("Nema apartmana!");
            if (!clients.Any())
                MessageBox.Show("Nema klijenata!");
            if (!itemCatalog.Any())
                MessageBox.Show("Nema stavki kataloga!");
            if (!invoices.Any())
                MessageBox.Show("Nema faktura!");

            // Kreirajte BindingSource
            BindingSource bindingSource = new BindingSource();

            foreach (var a in apartments)
            {
                // Povezivanje sa klijentom putem Invoice (klijent je povezan sa fakturom)
                var invoice = invoices.Where(x => x.ClientId == a.ClientId).FirstOrDefault(); // Pretpostavljamo da je ClientId vezan za Apartment i Invoice

                // Povezivanje sa stavkama fakture putem InvoiceId
                var apartmentInvoiceItems = invoiceItems.Where(x => x.InvoiceId == invoice?.Id).ToList();

                foreach (var invoiceItem in apartmentInvoiceItems)
                {
                    // Povezivanje sa stavkom kataloga putem ItemCatalogId
                    var item = itemCatalog.Where(ic => ic.Id == invoiceItem.ItemCatalogId).FirstOrDefault();

                    var client = invoice != null ? clients.Where(c => c.Id == invoice.ClientId).FirstOrDefault() : null;

                    var dataItem = new
                    {
                         ApartmanId = a.Id,
                        // BrojApartmana = a.Number,
                        Area = a.Area,
                        // ImeKlijenta = client != null ? client.Name : "N/A",
                        //PrezimeKlijenta = client != null ? client.Surname : "N/A",
                        ItemName = item != null ? item.Name : "N/A",
                        //Quantity = invoiceItem.Quantity,
                        PricePerUnit = invoiceItem.PricePerUnit,
                        //VAT = invoiceItem.VAT,
                        //VATRate = invoiceItem.VATRate,
                        Total = invoiceItem.PricePerUnit * a.Area,
                        Unit = item.Unit,
                        Number = invoiceItem.Number,
                       
                    };
                    txtCenaBezPDV.Text = dataItem.Total.ToString();
                    // Dodajte red u BindingSource
                    bindingSource.Add(dataItem);
                }
            }
                        

            // Povežite BindingSource sa DataGridView
            dataGridView1.DataSource = bindingSource;
            dataGridView1.Refresh();
        }




        private void FillComboBoxs()
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
            if (cmbClients.Items.Count > 0)
            {
                cmbClients.SelectedIndex = 0;
            }

            var buildings = buildingRepository.GetAll();
            cmbBuilding.DataSource = buildings;
            cmbBuilding.DisplayMember = "Address";
            cmbBuilding.ValueMember = "Id";
            if (cmbBuilding.Items.Count > 0)
            {
                cmbBuilding.SelectedIndex = 0;
            }

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

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

            dataGridView1.Columns["Delete"].DisplayIndex = dataGridView1.Columns.Count - 1;
            // Sakrij kolone koje nisu potrebne
            if (dataGridView1.Columns.Contains("ApartmanId"))
                dataGridView1.Columns["ApartmanId"].Visible = false;

            if (dataGridView1.Columns.Contains("KlijentId"))
                dataGridView1.Columns["KlijentId"].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    var invoiceItemId = (Guid)dataGridView1.Rows[e.RowIndex].Cells["ApartmanId"].Value;
                    repository.Delete(invoiceItemId);
                    BindingSource bindingSource = (BindingSource)dataGridView1.DataSource;
                    bindingSource.RemoveAt(e.RowIndex);
                    dataGridView1.Refresh();
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
     

        }
    }
}
