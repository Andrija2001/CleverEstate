using CleverEstate.Forms.InvoiceItems;
using CleverEstate.Forms.Invoices;
using CleverEstate.Models;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Classes;
using CleverEstate;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System;
using Xceed.Document.NET;
using Xceed.Words.NET;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using Font = System.Drawing.Font;
namespace CleverEstate.Forms.Invoices
{
    public partial class FrmInvoice : Form
    {
        private Form1 parentForm;
        private InvoiceRepository repository;
        private ItemCatalogRepository itemCatalogRepository;
        private ClientRepository clientRepository;
        private BuildingRepository buildingRepository;
        private ApartmentRepository apartmentRepository;
        private InvoiceItemRepository invoiceItemRepository;
        private Guid currentInvoiceId = Guid.Empty;
        private BindingSource bindingSource1 = new BindingSource();

        public FrmInvoice(Form1 parentForm, InvoiceRepository invoiceRepository)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            itemCatalogRepository = new ItemCatalogRepository(new DataDbContext());
            repository = new InvoiceRepository(new DataDbContext());
            clientRepository = new ClientRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            apartmentRepository = new ApartmentRepository(new DataDbContext());
            invoiceItemRepository = new InvoiceItemRepository(new DataDbContext());
            bindingSource1.DataSource = typeof(InvoiceItem);
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            repository = invoiceRepository;
            dataGridView1.Font = new Font("Times New Roman", 14);

            LoadNewInvoiceItems();
           
        }
        

        public void LoadNewInvoiceItems()
        {
            var invoiceItems = invoiceItemRepository.GetAll();
            var itemCatalogs = itemCatalogRepository.GetAll();
            var apartments = apartmentRepository.GetAll();
            var invoices = repository.GetAll();

            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("ItemCatalogid", typeof(Guid));
            table.Columns.Add("InvoiceItemId", typeof(Guid));
            table.Columns.Add("Broj", typeof(int));
            table.Columns.Add("Vrsta dobara", typeof(string));
            table.Columns.Add("Cena po jedinici", typeof(decimal));
            table.Columns.Add("Površina", typeof(decimal));
            table.Columns.Add("Jedinica Mere", typeof(string));
            table.Columns.Add("Ukupna Cena", typeof(decimal));
            table.Columns.Add("Količina", typeof(int));
            table.Columns.Add("PDV", typeof(decimal));
            table.Columns.Add("Stopa PDV", typeof(string));

            var query = from invoiceItem in invoiceItems
                        join itemCatalog in itemCatalogs on invoiceItem.ItemCatalogId equals itemCatalog.Id
                        join invoice in invoices on invoiceItem.InvoiceId equals invoice.Id
                        join apartment in apartments on invoice.ClientId equals apartment.ClientId
                        where invoice.Id == currentInvoiceId
                        select new
                        {
                            Id = invoiceItem.Id,
                            catalogid = invoiceItem.ItemCatalogId,
                            Invoiceitemid = invoiceItem.Id,
                            ItemName = itemCatalog.Name,
                            PricePerUnit = invoiceItem.PricePerUnit,
                            Area = apartment.Area,
                            Unit = itemCatalog.Unit,
                            Quantity = invoiceItem.Quantity,
                            VAT = invoiceItem.VAT * 10,
                            VATRate = invoiceItem.VATRate,
                            Total = invoiceItem.PricePerUnit * invoiceItem.Quantity * apartment.Area,
                        };

            int brojac = 1;
            foreach (var item in query)
            {
                table.Rows.Add(
                    item.Id,
                    item.catalogid,
                    item.Invoiceitemid,
                    brojac++,
                    item.ItemName,
                    item.PricePerUnit,
                item.Area,
                item.Unit,
                    item.Total,
                    item.Quantity,
                    item.VAT,
                item.VATRate
                );
            }
            dataGridView1.DataSource = table;
            dataGridView1.Refresh();

            UpdateTotalSum();
            IzracunajProcenat();
        }

        public FrmInvoice(Form1 parentForm, Guid invoiceId)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            itemCatalogRepository = new ItemCatalogRepository(new DataDbContext());
            repository = new InvoiceRepository(new DataDbContext());
            clientRepository = new ClientRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            apartmentRepository = new ApartmentRepository(new DataDbContext());
            invoiceItemRepository = new InvoiceItemRepository(new DataDbContext());
            currentInvoiceId = invoiceId;
            bindingSource1.DataSource = typeof(InvoiceItem);
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            LoadInvoiceItems(currentInvoiceId);
        }
        private void FrmInvoice_Load(object sender, EventArgs e)
        {
            if (currentInvoiceId == Guid.Empty)
            {
                var invoices = repository.GetAll();
                if (invoices.Any())
                    currentInvoiceId = invoices.First().Id;
                else
                    currentInvoiceId = Guid.Empty;
            }
            UpdateTotalSum();
            FillComboBox();
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
            AutoSizeComboBox(cmbClients);
            var apartments = apartmentRepository.GetAll();
            cmbApartmants.DataSource = apartments;
            cmbApartmants.DisplayMember = "Number";
            cmbApartmants.ValueMember = "Id";
            cmbApartmants.SelectedIndex = cmbApartmants.Items.Count > 0 ? 0 : -1;
            AutoSizeComboBox(cmbApartmants);
        }
        private void AutoSizeComboBox(ComboBox comboBox)
        {
            int maxWidth = 0;
            Graphics g = comboBox.CreateGraphics();
            Font font = comboBox.Font;

            foreach (var item in comboBox.Items)
            {
                string text = comboBox.GetItemText(item);
                int width = (int)g.MeasureString(text, font).Width;
                if (width > maxWidth)
                    maxWidth = width;
            }
            comboBox.Width = maxWidth + 25;
        }
        public void LoadInvoiceItems(Guid selectedInvoiceId)
        {
            var invoiceItems = invoiceItemRepository.GetAll();
            var itemCatalogs = itemCatalogRepository.GetAll();
            var apartments = apartmentRepository.GetAll();
            var invoices = repository.GetAll();
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("ItemCatalogid", typeof(Guid));
            table.Columns.Add("InvoiceItemId", typeof(Guid));
            table.Columns.Add("Number", typeof(int));
            table.Columns.Add("ItemName", typeof(string));
            table.Columns.Add("PricePerUnit", typeof(decimal));
            table.Columns.Add("Area", typeof(decimal));
            table.Columns.Add("Unit", typeof(string));
            table.Columns.Add("Total", typeof(decimal));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("VAT", typeof(decimal));
            table.Columns.Add("VATRate", typeof(string));

            var query = from invoiceItem in invoiceItems
                        join itemCatalog in itemCatalogs on invoiceItem.ItemCatalogId equals itemCatalog.Id
                        join invoice in invoices on invoiceItem.InvoiceId equals invoice.Id
                        join apartment in apartments on invoice.ClientId equals apartment.ClientId
                        where invoice.Id == selectedInvoiceId
                        select new
                        {
                            Id = invoiceItem.Id,
                            catalogid = invoiceItem.ItemCatalogId,
                            Invoiceitemid = invoiceItem.Id,
                            ItemName = itemCatalog.Name,
                            PricePerUnit = invoiceItem.PricePerUnit,
                            Area = apartment.Area,
                            Unit = itemCatalog.Unit,
                            Quantity = invoiceItem.Quantity,
                            VAT = invoiceItem.VAT * 10,
                            VATRate = invoiceItem.VATRate,
                            Total = invoiceItem.PricePerUnit * invoiceItem.Quantity * apartment.Area,
                        };
            int brojac = 1;
            foreach (var item in query)
            {
                table.Rows.Add(
                    item.Id,
                    item.catalogid,
                    item.Invoiceitemid,
                    brojac++,
                    item.ItemName,
                    item.PricePerUnit,
                    item.Area,
                    item.Unit,
                    item.Total,
                    item.Quantity,
                    item.VAT,
                    item.VATRate
                );
            }
            dataGridView1.DataSource = table;
            dataGridView1.Refresh();
            UpdateTotalSum();
            IzracunajProcenat();
        }
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;
                var columnName = dgv.Columns[e.ColumnIndex].DataPropertyName;

                if (columnName == "Quantity")
                {
                    string novaVrednostStr = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                    if (!int.TryParse(novaVrednostStr, out int novaVrednost))
                    {
                        MessageBox.Show("Nevalidan unos za količinu.");
                        return;
                    }

                    var dataRowView = dgv.Rows[e.RowIndex].DataBoundItem as DataRowView;
                    if (dataRowView != null)
                    {
                        dataRowView["Quantity"] = novaVrednost;
                        SaveQuantityChange(dataRowView);
                        LoadInvoiceItems(currentInvoiceId);
                    }
                }
            }
        }
        private void UpdateTotalSum()
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                decimal suma = SumirajKolonu(dt, "Total");
                txtCenaBezPDV.Text = suma.ToString("N2");
            }
        }
        private decimal SumirajKolonu(DataTable dt, string columnName)
        {
            decimal suma = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] != DBNull.Value)
                {
                    suma += Convert.ToDecimal(row[columnName]);
                }
            }
            return suma;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!dataGridView1.Columns.Contains("Delete"))
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Obriši",
                    UseColumnTextForButtonValue = true
                };
                dataGridView1.Columns.Add(btnDelete);
            }

            dataGridView1.Columns["Delete"].DisplayIndex = dataGridView1.Columns.Count - 1;

            if (dataGridView1.Columns.Contains("Id"))
                dataGridView1.Columns["Id"].Visible = false;

            if (dataGridView1.Columns.Contains("InvoiceId"))
                dataGridView1.Columns["InvoiceId"].Visible = false;
            if (dataGridView1.Columns.Contains("ItemCatalogId"))
                dataGridView1.Columns["ItemCatalogId"].Visible = false;
            if (dataGridView1.Columns.Contains("InvoiceItemId"))
                dataGridView1.Columns["InvoiceItemId"].Visible = false;
      
        }
        private void SaveQuantityChange(DataRowView dataRowView)
        {
            if (dataRowView == null)
                return;

            Guid id = (Guid)dataRowView["Id"];
            int newQuantity = (int)dataRowView["Quantity"];

            InvoiceItem invoiceItem = invoiceItemRepository.GetById(id);
            if (invoiceItem != null)
            {
                invoiceItem.Quantity = newQuantity;
                invoiceItemRepository.Update(invoiceItem);
                invoiceItemRepository.Save();
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                var itemId = (Guid)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                invoiceItemRepository.Delete(itemId);
                LoadInvoiceItems(currentInvoiceId);
                UpdateTotalSum();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var selectedClient = cmbClients.SelectedValue as Guid?;
            var selectedBuilding = cmbApartmants.SelectedValue as Guid?;

            if (selectedClient.HasValue && selectedBuilding.HasValue)
            {
                if (currentInvoiceId == Guid.Empty)
                {
                    var newInvoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        ClientId = selectedClient.Value,
                    };

                    repository.Insert(newInvoice);
                    repository.Save(); 

                    currentInvoiceId = newInvoice.Id;
                }

                var invoice = repository.GetById(currentInvoiceId);
                if (invoice == null)
                {
                    MessageBox.Show("Greška: faktura nije pronađena.");
                    return;
                }
                FrmAddInvoiceItem frmAddInvoiceItem = new FrmAddInvoiceItem(
                    this,
                    repository,
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
                MessageBox.Show("Molimo izaberite adresu.");
                return;
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePickerDate.Value;

            int month = selectedDate.Month;
            if (month >= 1 && month <= 12)
            {
                cmbMonth.SelectedIndex = month - 1;
            }
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePickerPaymentDeadline.Value;

            int month = selectedDate.Month;
            if (month >= 1 && month <= 12)
            {
                cmbMonth.SelectedIndex = month - 1;
            }
        }
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePickerInvoiceDate.Value;
            int month = selectedDate.Month;
            if (month >= 1 && month <= 12)
            {
                cmbMonth.SelectedIndex = month - 1;
            }
        }
        private void IzracunajProcenat()
        {
            decimal broj = decimal.Parse(txtCenaBezPDV.Text);
            decimal procenat = 20m;
            decimal rezultat = broj * (procenat / 100);
            txtUkupnaCena.Text = rezultat.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            DateTime date = dateTimePickerDate.Value;
            DateTime paymentDeadline = dateTimePickerPaymentDeadline.Value;
            DateTime invoiceDate = dateTimePickerInvoiceDate.Value;
            string month = cmbMonth.SelectedItem?.ToString() ?? "";
            string period = txtPeriod.Text;
            string invoiceNumber = txtInvoiceNumber.Text;
            if (string.IsNullOrEmpty(invoiceNumber))
            {
                MessageBox.Show("Unesite broj racuna.");
                return;
            }
            if (string.IsNullOrEmpty(period))
            {
                MessageBox.Show("Unesite period za racun");
            }
            Guid invoiceId;

            if (currentInvoiceId == Guid.Empty)
            {
                invoiceId = Guid.NewGuid();
                currentInvoiceId = invoiceId;
            }
            else
            {
                invoiceId = currentInvoiceId;
            }
            Guid clientId = Guid.Parse(cmbClients.SelectedValue.ToString());
            SaveInvoice(invoiceId, date, paymentDeadline, invoiceDate, month, period, invoiceNumber, clientId);
            SaveInvoiceItems(invoiceId);
            currentInvoiceId = invoiceId;


        }

        private void SaveInvoiceItems(Guid invoiceId)
        {
            invoiceItemRepository.DeleteByInvoiceId(invoiceId);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var row = dataGridView1.Rows[i];
                if (row.IsNewRow) continue;
                var item = new InvoiceItem
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoiceId,
                    ItemCatalogId = Guid.Parse(row.Cells["ItemCatalogId"].Value.ToString()),
                    PricePerUnit = Convert.ToDecimal(row.Cells["PricePerUnit"].Value),
                    Quantity = int.Parse(row.Cells["Quantity"].Value.ToString()),
                    VAT = Convert.ToDecimal(row.Cells["VAT"].Value),
                    VATRate = row.Cells["VATRate"].Value.ToString(),
                };
                invoiceItemRepository.Insert(item);
                invoiceItemRepository.Save();
            }
        }

        private void SaveInvoice(Guid invoiceId, DateTime date, DateTime paymentDeadline, DateTime invoiceDate, string month, string period, string invoiceNumber, Guid clientId)
        {
            try
            {
                var existing = repository.GetById(invoiceId);
                if (existing != null)
                {
                    existing.Date = date;
                    existing.PaymentDeadline = paymentDeadline;
                    existing.InvoiceDate = invoiceDate;
                    existing.Month = month;
                    existing.Period = period;
                    existing.InvoiceNumber = invoiceNumber;
                    existing.ClientId = clientId;

                    repository.Update(existing);
                    repository.Save();

                }
                else
                {
                    Invoice invoice = new Invoice
                    {
                        Id = invoiceId,
                        Date = date,
                        PaymentDeadline = paymentDeadline,
                        InvoiceDate = invoiceDate,
                        Month = month,
                        Period = period,
                        InvoiceNumber = invoiceNumber,
                        ClientId = clientId,
                    };
                    repository.Insert(invoice);
                    repository.Save();
                }
              
                parentForm.bindingSource1.ResetBindings(false);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Greška prilikom čuvanja fajla");
            }
        }
        public void SetInvoiceData(Guid invoiceId, DateTime date, string month, DateTime paymentDeadline, string period, string invoiceNumber, DateTime invoiceDate)
        {
            currentInvoiceId = invoiceId;
            dateTimePickerDate.Value = date;
            month = month.Trim();
            for (int i = 0; i < cmbMonth.Items.Count; i++)
            {
                if (cmbMonth.Items[i].ToString().Trim().Equals(month, StringComparison.OrdinalIgnoreCase))
                {
                    cmbMonth.SelectedIndex = i;
                    break;
                }
            }
            dateTimePickerPaymentDeadline.Value = paymentDeadline;
            txtPeriod.Text = period;
            txtInvoiceNumber.Text = invoiceNumber;
            dateTimePickerInvoiceDate.Value = invoiceDate;
        }
        private void btnStampaj_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedClientId = cmbClients.SelectedValue as Guid?;
                string clientName = cmbClients.Text;
                int apartmentnumber = int.Parse(cmbApartmants.Text);
                if (!selectedClientId.HasValue)
                {
                    MessageBox.Show("Izaberite klijenta.");
                    return;
                }
                string clientAddress = "";
                string clientCity = "";
                using (var context = new DataDbContext())
                {
                    var apartmentWithBuilding = context.Apartments
                        .Where(a => a.ClientId == selectedClientId.Value)
                        .Join(context.Buildings,
                              a => a.BuildingId,
                              b => b.Id,
                              (a, b) => new { Apartment = a, Building = b })
                        .FirstOrDefault();

                    if (apartmentWithBuilding != null)
                    {
                        clientAddress = apartmentWithBuilding.Building.Address ?? "";
                        clientCity = apartmentWithBuilding.Building.City ?? "";
                    }
                }
                int clientPIB = 0;
                string clientBankAccount = "";
                using (var context = new DataDbContext())
                {
                    var client = context.Clients.FirstOrDefault(c => c.Id == selectedClientId.Value);
                    if (client != null)
                    {
                        clientPIB = client.PIB;
                        clientBankAccount = client.BankAccount;
                    }
                }
                var itemsForPrinting = new List<(string ItemName, string period, DateTime date, DateTime invoiceDate, string UnitOfMeasure, int Quantity, decimal PricePerUnit, decimal VAT, string VATRate, decimal Total, string Number)>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    var drv = row.DataBoundItem as DataRowView;
                    if (drv != null)
                    {
                        Guid itemCatalogId = drv.Row.Field<Guid>("ItemCatalogid");
                        var itemcatalog = itemCatalogRepository.GetById(itemCatalogId);

                        if (itemcatalog == null)
                        {
                            MessageBox.Show("Nije pronadjena stavka racuna");
                            return;
                        }
                        itemsForPrinting.Add((
                            ItemName: itemcatalog.Name,
                            period: txtPeriod.Text,
                            date: dateTimePickerDate.Value,
                            invoiceDate: dateTimePickerInvoiceDate.Value,
                            UnitOfMeasure: itemcatalog.Unit,
                            Quantity: drv.Row.Field<int>("Quantity"),
                            PricePerUnit: drv.Row.Field<decimal>("PricePerUnit"),
                            VAT: drv.Row.Field<decimal>("VAT"),
                            VATRate: drv.Row["VATRate"]?.ToString() ?? string.Empty,
                            Total: drv.Row.Field<decimal>("Total"),
                            Number: drv.Row.Field<int>("Number").ToString()
                        ));
                    }
                }
                if (itemsForPrinting.Count == 0)
                {
                    return;
                }
            
                string tempPath = Path.Combine(Path.GetTempPath(), "Racun.docx");
                using (var doc = DocX.Create(tempPath))
                {
                    doc.InsertParagraph("RAČUN")
                       .FontSize(16)
                       .Bold()
                       .Alignment = Alignment.center;
                    doc.InsertParagraph(Environment.NewLine);
                    string organizerText = "Prodavac\n" +
                                           "SZ" + clientAddress + "\n" +
                                           "PIB:" + clientPIB + "\n" +
                                           "MB: 67700872" + "\n" +
                                           "Tekući račun:" + clientBankAccount + "\n";
                    string clientText = "Klijent\n" +
                                        clientName + "\n" +
                                        clientAddress + "\n" +
                                        "11000" + clientCity + "\n" +
                                        "Za objekat SZ: " + clientAddress + "stan_" + apartmentnumber + "_";
                    var contactTable = doc.AddTable(1, 2);
                    contactTable.Alignment = Alignment.center;
                    contactTable.Design = TableDesign.None;

                    contactTable.Rows[0].Cells[0].Paragraphs[0].Append(organizerText).FontSize(10).Alignment = Alignment.left;
                    contactTable.Rows[0].Cells[1].Paragraphs[0].Append(clientText).FontSize(10).Alignment = Alignment.right;
                    doc.InsertTable(contactTable);
                    doc.InsertParagraph(Environment.NewLine);
                    for (int i = 0; i < itemsForPrinting.Count; i++)
                    {
                        var item = itemsForPrinting[i];
                        doc.InsertParagraph("Mesto i datum izdavanja: " + item.invoiceDate.ToShortDateString()).Alignment = Alignment.left;
                        doc.InsertParagraph("Datum prometa: " + item.date.ToShortDateString()).Alignment = Alignment.left;
                        doc.InsertParagraph("Period: " + item.period).Alignment = Alignment.left;
                        break;
                    }
                    doc.InsertParagraph(Environment.NewLine);
                    Table table = doc.AddTable(itemsForPrinting.Count + 1, 9);
                    table.Alignment = Alignment.center;
                    table.Design = TableDesign.TableGrid;
                    table.Rows[0].Cells[0].Paragraphs[0].Append("Broj").Bold();
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Vrsta dobara").Bold();
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Jedinica mere").Bold();
                    table.Rows[0].Cells[3].Paragraphs[0].Append("Količina").Bold();
                    table.Rows[0].Cells[4].Paragraphs[0].Append("Cena po jedinici").Bold();
                    table.Rows[0].Cells[5].Paragraphs[0].Append("Osnovica").Bold();
                    table.Rows[0].Cells[6].Paragraphs[0].Append("Stopa PDV").Bold();
                    table.Rows[0].Cells[7].Paragraphs[0].Append("PDV").Bold();
                    table.Rows[0].Cells[8].Paragraphs[0].Append("Ukupna naknada").Bold();
                    decimal totalInvoiceAmount = 0;
                    for (int i = 0; i < itemsForPrinting.Count; i++)
                    {
                        var item = itemsForPrinting[i];
                        int rowIndex = i + 1;
                        var osnovica = item.Quantity * item.PricePerUnit;
                        table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(item.Number ?? "");
                        table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(item.ItemName);
                        table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(item.UnitOfMeasure.ToString());
                        table.Rows[rowIndex].Cells[3].Paragraphs[0].Append(item.Quantity.ToString("N2"));
                        table.Rows[rowIndex].Cells[4].Paragraphs[0].Append(item.PricePerUnit.ToString("N2"));
                        table.Rows[rowIndex].Cells[5].Paragraphs[0].Append(osnovica.ToString("N2"));
                        table.Rows[rowIndex].Cells[6].Paragraphs[0].Append(item.VATRate.ToString());
                        table.Rows[rowIndex].Cells[7].Paragraphs[0].Append(item.VAT.ToString("N2"));
                        table.Rows[rowIndex].Cells[8].Paragraphs[0].Append(item.Total.ToString("N2"));
                        totalInvoiceAmount += osnovica;
                    }
                    doc.InsertTable(table);
                    Table summaryTable = doc.AddTable(3, 2);
                    summaryTable.Alignment = Alignment.right;
                    summaryTable.Design = TableDesign.TableGrid;
                    summaryTable.Rows[0].Cells[0].Paragraphs[0].Append("OSNOVICA:").Bold();
                    summaryTable.Rows[1].Cells[0].Paragraphs[0].Append("UKUPNO PDV:").Bold();
                    summaryTable.Rows[2].Cells[0].Paragraphs[0].Append("Ukupno sa PDV-om:").Bold();
                    decimal osnovicaSum = totalInvoiceAmount;
                    decimal ukupnoPDV = 0.20m;
                    decimal ukupnoSaPDV = osnovicaSum * 0.20m;
                    summaryTable.Rows[0].Cells[1].Paragraphs[0].Append(osnovicaSum.ToString("N2"));
                    summaryTable.Rows[1].Cells[1].Paragraphs[0].Append(ukupnoPDV.ToString("N2"));
                    summaryTable.Rows[2].Cells[1].Paragraphs[0].Append(ukupnoSaPDV.ToString("N2"));
                    foreach (var row in summaryTable.Rows)
                    {
                        row.Cells[0].Width = 150;
                        row.Cells[1].Width = 150;
                    }
                    doc.InsertParagraph(Environment.NewLine);
                    doc.InsertTable(summaryTable);
                    doc.InsertParagraph(Environment.NewLine);
                    doc.InsertParagraph("-Sve cene su izražene u valuti: RSD").Alignment = Alignment.left;
                    doc.InsertParagraph("-PR Vaš upravnik nije u sistemu PDV-a").Alignment = Alignment.left;
                    doc.InsertParagraph("-Račun je validan bez pečata i potpisa").Alignment = Alignment.left;
                    doc.InsertParagraph("-U pozivu na broj navedite broj računa").Alignment = Alignment.left;
                    string basePath = Application.StartupPath;
                    doc.InsertParagraph(Environment.NewLine);
                    string relativePath = @"..\..\Uplatnica.jpg";
                    string imagePath = Path.GetFullPath(Path.Combine(basePath, relativePath));
                    if (File.Exists(imagePath))
                    {
                        var image = doc.AddImage(imagePath);
                        var picture = image.CreatePicture();
                        picture.Width = 380;
                        picture.Height = 180;
                        doc.InsertParagraph().AppendPicture(picture).Alignment = Alignment.left;
                    }
                    else
                    {
                        MessageBox.Show("Slika nije pronađena na putanji: " + imagePath);
                    }
                    doc.Save();
                }
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = Path.Combine(Path.GetTempPath(), "Racun.docx"),
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Greška prilikom štampanja računa");
            }
        }

       
    }
}
