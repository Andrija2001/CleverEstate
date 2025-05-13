using CleverEstate.Forms.Apartments;
using CleverEstate.Forms.InvoiceItems;
using CleverEstate.Models;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate.Forms.Invoices
{
    public partial class FrmInvoice : Form
    {
        private InvoiceItemRepository invoiceItemRepository;
        private ApartmentRepository apartmentRepository;
        private ClientRepository clientRepository;
        private BuildingRepository buildingRepository;
        private ItemCatalogRepository itemCatalogRepository;
        private InvoiceRepository repository;
        private Button addNewRowButton = new Button();
        private Panel buttonPanel = new Panel();
        public BindingSource bindingSource1 = new BindingSource();
        Font font = new Font("Arial", 12);
        public FrmInvoice()
        {
            itemCatalogRepository = new ItemCatalogRepository(new DataDbContext());
            repository = new InvoiceRepository(new DataDbContext());
            clientRepository = new ClientRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            apartmentRepository = new ApartmentRepository(new DataDbContext());
            invoiceItemRepository = new InvoiceItemRepository(new DataDbContext());
            InitializeComponent();
            bindingSource1.DataSource = typeof(Invoice);
        }

        private void FrmInvoice_Load(object sender, EventArgs e)
        {
            FillComboBox();
            LoadInvoices();
        }

        private void FillComboBox()
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
            cmbClients.SelectedIndex = cmbClients.Items.Count > 0 ? 0 : -1;
            var apartments = apartmentRepository.GetAll();
            cmbApartmants.DataSource = apartments;
            cmbApartmants.DisplayMember = "Number";
            cmbApartmants.ValueMember = "Id";
            cmbApartmants.SelectedIndex = cmbApartmants.Items.Count > 0 ? 0 : -1;
        }

        public void LoadInvoices()
        {
            var invoiceItems = invoiceItemRepository.GetAll();
            var itemCatalog = itemCatalogRepository.GetAll();
            var invoices = repository.GetAll();
            var apartments = apartmentRepository.GetAll();
            int brojac = 0;
            BindingSource bindingSource = new BindingSource();
            List<dynamic> dataList = new List<dynamic>();
            foreach (var invoice in invoices)
            {
                var clientApartments = apartments
                    .Where(a => a.ClientId == invoice.ClientId)
                    .ToList();

                foreach (var invoiceItem in invoiceItems.Where(x => x.InvoiceId == invoice.Id))
                {
                    var item = itemCatalog.FirstOrDefault(ic => ic.Id == invoiceItem.ItemCatalogId);
                    foreach (var apartment in clientApartments)
                    {
                        decimal totalPrice = invoiceItem.PricePerUnit * apartment.Area;  

                        var dataItem = new
                        {
                            Id= invoiceItem.Id,
                            Number = brojac++,                 
                            ItemName = item != null ? item.Name : "N/A", 
                            PricePerUnit = invoiceItem.PricePerUnit,      
                            Area = apartment.Area,                        
                            Unit = invoiceItem.Number,                    
                            Total = totalPrice                           
                        };

                        // Dodaj stavku u listu
                        dataList.Add(dataItem);
                    }
                }
            }
            var sortedDataList = dataList.OrderBy(item => item.Unit).ToList();
            foreach (var dataItem in sortedDataList)
            {
                bindingSource.Add(dataItem);
            }
            dataGridView1.DataSource = bindingSource;
            dataGridView1.Refresh();
        }
        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Size = new Size(500, 250);
            dataGridView1.Dock = DockStyle.Fill;
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!dataGridView1.Columns.Contains("Delete"))
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                };
                dataGridView1.Columns.Add(btnDelete);
            }
            dataGridView1.Columns["Delete"].DisplayIndex = dataGridView1.Columns.Count - 1;
            if (dataGridView1.Columns.Contains("Id"))
                dataGridView1.Columns["Id"].Visible = false;

            if (dataGridView1.Columns.Contains("InvoiceId"))
                dataGridView1.Columns["InvoiceId"].Visible = false;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                var itemId = (Guid)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                invoiceItemRepository.Delete(itemId);
                LoadInvoices();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var selectedClient = cmbClients.SelectedValue as Guid?;
            var selectedBuilding = cmbApartmants.SelectedValue as Guid?;
            if (selectedClient.HasValue && selectedBuilding.HasValue)
            {
                var invoice = repository.GetAll().FirstOrDefault(i => i.ClientId == selectedClient.Value);
                if (invoice == null)
                {
                    invoice = new Invoice
                    {
                        ClientId = selectedClient.Value,
                        Id = Guid.NewGuid(),
                    };
                   repository.Insert(invoice);
                }
                FrmAddInvoiceItem frmAddInvoiceItem = new FrmAddInvoiceItem(
                    this,
                    invoiceItemRepository,
                    itemCatalogRepository,
                    clientRepository,
                    selectedBuilding.Value,
                    selectedClient.Value,
                    invoice.Id
                );
                frmAddInvoiceItem.ShowDialog();
            }
            else
            {
                MessageBox.Show("Molimo izaberite adresu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmbClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filtriraj();
        }
        public void Filtriraj()
        {
           var selectedClient = cmbClients.SelectedItem as dynamic;
                if (selectedClient == null) return;
                string clientName = selectedClient.FullName;
                var klijent = clientRepository.GetAll().FirstOrDefault(k => (k.Name + " " + k.Surname) == clientName);
                if (klijent == null) return;
                var apartmentsForClient = apartmentRepository.GetAll().Where(a => a.ClientId == klijent.Id).ToList();
                var invoicesForClient = repository.GetAll().Where(i => i.ClientId == klijent.Id).ToList();
                int brojac = 0;
                 List<dynamic> dataList = new List<dynamic>();
                foreach (var invoice in invoicesForClient)
                {
                    foreach (var invoiceItem in invoiceItemRepository.GetAll().Where(ii => ii.InvoiceId == invoice.Id))
                    {
                        var item = itemCatalogRepository.GetAll().FirstOrDefault(ic => ic.Id == invoiceItem.ItemCatalogId);

                        foreach (var apartment in apartmentsForClient)
                        {
                        decimal totalPrice = invoiceItem.PricePerUnit * apartment.Area;
                            var dataItem = new
                            {
                               Id = invoiceItem.Id,
                                Number = brojac++,
                                ItemName = item != null ? item.Name : "N/A",
                                PricePerUnit = invoiceItem.PricePerUnit,
                                Area = apartment.Area,
                                Unit = invoiceItem.Number,
                                Total = totalPrice
                            };
                            dataList.Add(dataItem);
                        }
                    }
                }
               var sortedDataList = dataList.OrderBy(item => item.Number).ToList();
                dataGridView1.DataSource = sortedDataList;
                dataGridView1.Refresh();
            }
        }
    }
