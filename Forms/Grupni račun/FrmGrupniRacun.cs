using CleverEstate.Models;
using CleverEstate.Services.Interface.Repository;
using CleverEstate.Services.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CleverEstate.Services.Classes;
using CleverEstate.Services.Classes.Repository;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Castle.Components.DictionaryAdapter.Xml;

namespace CleverEstate.Forms
{
    public partial class FrmGrupniRacun : Form
    {
        private readonly ApartmentRepository apartmentRepository;

        private readonly BuildingRepository buildingRepository;
        private readonly ClientRepository clientRepository;
        private readonly InvoiceRepository invoiceRepository;
        private readonly InvoiceItemRepository invoiceItemRepository;
        private readonly ItemCatalogRepository itemCatalogRepository;
        public Form1 _mainForm;
        public FrmGrupniRacun(Form1 mainForm, BuildingRepository buildingRepo,InvoiceItemRepository invoiceItemRepository,  ItemCatalogRepository itemCatalogRepository,  ApartmentRepository repo, ClientRepository clientRepo, InvoiceRepository invoiceRepo)
        {
            InitializeComponent();
            this.itemCatalogRepository = itemCatalogRepository;
            this.invoiceItemRepository = invoiceItemRepository;
            buildingRepository = buildingRepo;
            clientRepository = clientRepo;
            invoiceRepository = invoiceRepo;
            apartmentRepository = repo;
            _mainForm = mainForm;
            LoadBuildings();
        }

        private void LoadBuildings()
        {
            var buildings = buildingRepository.GetAll().ToList();
            cmbAdresa.DataSource = buildings;
            cmbAdresa.DisplayMember = "Address";
            cmbAdresa.ValueMember = "Id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedBuildingId = (Guid)cmbAdresa.SelectedValue;
            var mesec = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1);
          //  KreirajRacuneZaZgradu(selectedBuildingId, mesec);
            DodajRacuneUBazu(selectedBuildingId,mesec);
            _mainForm.LoadData();

        }
        private void DodajRacuneUBazu(Guid buildingId, DateTime mesec)
        {
            var building = buildingRepository.GetById(buildingId);
            if (building == null)
            {
                MessageBox.Show("Zgrada nije pronađena.");
                return;
            }

            var buildingApartments = apartmentRepository.GetAll().Where(a => a.BuildingId == buildingId).ToList();
            if (!buildingApartments.Any())
            {
                MessageBox.Show("Zgrada nema stanove.");
                return;
            }

            DateTime selectedDate = mesec;
            DateTime periodStart = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            DateTime periodEnd = periodStart.AddMonths(1).AddDays(-1);
            string periodText = $"{periodStart:dd.MM.yyyy} - {periodEnd:dd.MM.yyyy}";

            var itemCatalogList = itemCatalogRepository.GetAll().ToList();

            foreach (var apartment in buildingApartments)
            {
                var client = clientRepository.GetById(apartment.ClientId);
                if (client == null) continue;

                bool postojiRacun = invoiceRepository.GetAll().Any(inv =>
                     inv.ClientId == client.Id &&
                     inv.InvoiceDate.Year == mesec.Year &&
                     inv.InvoiceDate.Month == mesec.Month);

                if (postojiRacun)
                {
                    MessageBox.Show("Taj racun vec postoji");
                    return;
                }

                var newInvoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    ClientId = client.Id,
                    Date = DateTime.Now,
                    InvoiceNumber = client.BankAccount,
                    InvoiceDate = mesec,
                    Month = mesec.ToString("MMMM yyyy"),
                    Period = periodText,
                    PaymentDeadline = DateTime.Now.AddDays(15),
                    InvoiceStatus = false,
                    InvoiceItems = new List<InvoiceItem>()
                };

                foreach (var catalogItem in itemCatalogList)
                {
                    int quantity = 1;
                    string vatRate = "20%";
                    decimal vatDecimal = 0.2m;

                    decimal pricePerUnit = catalogItem.PricePerUnit;
                    decimal vatAmount = pricePerUnit * quantity * vatDecimal;
                    decimal totalPrice = pricePerUnit * quantity + vatAmount;

                    var invoiceItem = new InvoiceItem
                    {
                        Id = Guid.NewGuid(),
                        InvoiceId = newInvoice.Id,
                        ItemCatalogId = catalogItem.Id,
                        Quantity = quantity,
                        PricePerUnit = pricePerUnit,
                        VATRate = vatRate,
                        VAT = vatAmount,
                        TotalPrice = totalPrice,
                        Number = (newInvoice.InvoiceItems.Count + 1).ToString()
                    };

                    newInvoice.InvoiceItems.Add(invoiceItem);
                }

                invoiceRepository.Insert(newInvoice);
            }
            invoiceRepository.Save(); // Dodaj ovo ako postoji u repozitorijumu


            MessageBox.Show("Računi su uspešno sačuvani u bazu.");
        }
        private void KreirajRacuneZaZgradu(Guid buildingId, DateTime mesec)
        {
            var building = buildingRepository.GetById(buildingId);
            if (building == null)
            {
                MessageBox.Show("Zgrada nije pronađena.");
                return;
            }

            var buildingApartments = apartmentRepository.GetAll().Where(a => a.BuildingId == buildingId).ToList();
            if (!buildingApartments.Any())
            {
                MessageBox.Show("Zgrada nema stanove.");
                return;
            }
            DateTime selectedDate = dateTimePicker1.Value;
            DateTime periodStart = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            DateTime periodEnd = periodStart.AddMonths(1).AddDays(-1);
            string periodText = $"{periodStart:dd.MM.yyyy} - {periodEnd:dd.MM.yyyy}";
            var itemCatalogList = itemCatalogRepository.GetAll().ToList();

            string fileName = $"GrupniRacuni.docx";
            string filePath = Path.Combine(Path.GetTempPath(), fileName);

            using (var doc = DocX.Create(filePath))
            {
                foreach (var apartment in buildingApartments)
                {
                    var client = clientRepository.GetById(apartment.ClientId);
                    if (client == null) continue;
                    var newInvoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        ClientId = client.Id,
                        Date = DateTime.Now,
                        InvoiceDate = mesec,
                        Month = mesec.ToString("MMMM yyyy"),
                        Period = periodText,
                        PaymentDeadline = DateTime.Now.AddDays(15),
                        InvoiceItems = new List<InvoiceItem>()
                    };

                    foreach (var catalogItem in itemCatalogList)
                    {
                        var invoiceIds = invoiceRepository.GetAll()
                           .Where(inv => inv.ClientId == client.Id &&
                                         inv.InvoiceDate.Year == mesec.Year &&
                                         inv.InvoiceDate.Month == mesec.Month)
                           .Select(inv => inv.Id)
                           .ToList();

                        var existingItem = invoiceItemRepository.GetAll()
                            .FirstOrDefault(ii =>
                                invoiceIds.Contains(ii.InvoiceId) &&
                                ii.ItemCatalogId == catalogItem.Id);

                        int quantity = existingItem?.Quantity ?? 1;
                        string vatRate = existingItem?.VATRate ?? "20%";

                        decimal vatDecimal = 0.2m;
                        if (vatRate.EndsWith("%") && decimal.TryParse(vatRate.TrimEnd('%'), out decimal parsedRate))
                            vatDecimal = parsedRate / 100m;

                        decimal pricePerUnit = catalogItem.PricePerUnit;
                        decimal vatAmount = pricePerUnit * quantity * vatDecimal;
                        decimal totalPrice = pricePerUnit * quantity + vatAmount;

                        var invoiceItem = new InvoiceItem
                        {
                            Id = Guid.NewGuid(),
                            InvoiceId = newInvoice.Id,
                            ItemCatalogId = catalogItem.Id,
                            Quantity = quantity,
                            PricePerUnit = pricePerUnit,
                            VATRate = vatRate,
                            VAT = vatAmount,
                            TotalPrice = totalPrice,
                            Number = (newInvoice.InvoiceItems.Count + 1).ToString()
                        };

                        newInvoice.InvoiceItems.Add(invoiceItem);
                    }
                    DodajRacunUKolekciju(doc,
                        client.Name + " " + client.Surname,
                        building.Address,
                        building.City,
                        client.PIB,
                        client.BankAccount,
                        apartment.Number,
                        newInvoice.InvoiceDate,
                        newInvoice.Period,
                        newInvoice.InvoiceItems,
                        itemCatalogList);

                    doc.InsertSectionPageBreak();
                }

                doc.Save();

                try
                {
                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true,
                        Verb = "open"
                    };
                    System.Diagnostics.Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri otvaranju dokumenta:\n" + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DodajRacunUKolekciju(DocX doc,string clientName,string clientAddress,string clientCity, int clientPIB,
            string clientBankAccount, int apartmentNumber, DateTime invoiceDate, string period, List<InvoiceItem> invoiceItems,
         List<ItemCatalog> itemCatalogList)
        {
            doc.InsertParagraph("RAČUN").FontSize(16).Bold().Alignment = Alignment.center;
            doc.InsertParagraph(Environment.NewLine);

            string organizerText = "Prodavac\n" +
                                   "SZ " + clientAddress + "\n" +
                                   "PIB: " + clientPIB + "\n" +
                                   "MB: 67700872\n" +
                                   "Tekući račun: " + clientBankAccount + "\n";

            string clientText = "Klijent\n" +
                                clientName + "\n" +
                                clientAddress + "\n" +
                                "11000 " + clientCity + "\n" +
                                $"Za objekat SZ: {clientAddress} stan {apartmentNumber}";

            var contactTable = doc.AddTable(1, 2);
            contactTable.Alignment = Alignment.center;
            contactTable.Design = TableDesign.None;
            contactTable.Rows[0].Cells[0].Paragraphs[0].Append(organizerText).FontSize(10).Alignment = Alignment.left;
            contactTable.Rows[0].Cells[1].Paragraphs[0].Append(clientText).FontSize(10).Alignment = Alignment.right;
            doc.InsertTable(contactTable);
            doc.InsertParagraph(Environment.NewLine);

            var firstItem = invoiceItems.First();
            doc.InsertParagraph("Mesto i datum izdavanja: " + invoiceDate.ToShortDateString()).Alignment = Alignment.left;
            doc.InsertParagraph("Datum prometa: " + invoiceDate.ToShortDateString()).Alignment = Alignment.left;
            doc.InsertParagraph("Period: " + period).Alignment = Alignment.left;
            doc.InsertParagraph(Environment.NewLine);

            Table table = doc.AddTable(invoiceItems.Count + 1, 9);
            table.Alignment = Alignment.center;
            table.Design = TableDesign.TableGrid;

            string[] headers = { "Broj", "Vrsta dobara", "Jedinica mere", "Količina", "Cena po jedinici", "Osnovica", "Stopa PDV", "PDV", "Ukupna naknada" };
            for (int i = 0; i < headers.Length; i++)
            {
                table.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();
            }

            decimal total = 0;
            for (int i = 0; i < invoiceItems.Count; i++)
            {
                var item = invoiceItems[i];
                int row = i + 1;
                var catalogItem = itemCatalogList.FirstOrDefault(c => c.Id == item.ItemCatalogId);

                table.Rows[row].Cells[0].Paragraphs[0].Append(item.Number);
                table.Rows[row].Cells[1].Paragraphs[0].Append(catalogItem?.Name ?? "Nepoznato");
                table.Rows[row].Cells[2].Paragraphs[0].Append(catalogItem?.Unit ?? "-");
                table.Rows[row].Cells[3].Paragraphs[0].Append(item.Quantity.ToString());
                table.Rows[row].Cells[4].Paragraphs[0].Append(item.PricePerUnit.ToString("N2"));
                table.Rows[row].Cells[5].Paragraphs[0].Append((item.Quantity * item.PricePerUnit).ToString("N2"));
                table.Rows[row].Cells[6].Paragraphs[0].Append(item.VATRate);
                table.Rows[row].Cells[7].Paragraphs[0].Append(item.VAT.ToString("N2"));
                table.Rows[row].Cells[8].Paragraphs[0].Append(item.TotalPrice.ToString("N2"));
                total += item.TotalPrice;
            }
            doc.InsertTable(table);
            doc.InsertParagraph(Environment.NewLine);
            var summaryTable = doc.AddTable(3, 2);
            summaryTable.Alignment = Alignment.right;
            summaryTable.Design = TableDesign.TableGrid;
            decimal totalVAT = invoiceItems.Sum(item => item.VAT);
            summaryTable.Rows[0].Cells[0].Paragraphs[0].Append("Osnovica + PDV").Bold();
            summaryTable.Rows[0].Cells[1].Paragraphs[0].Append(total.ToString("N2") + " RSD");
            summaryTable.Rows[1].Cells[0].Paragraphs[0].Append("Ukupan PDV").Bold();
            summaryTable.Rows[1].Cells[1].Paragraphs[0].Append(totalVAT.ToString("N2") + " RSD");
            summaryTable.Rows[2].Cells[0].Paragraphs[0].Append("Ukupna naknada").Bold();
            summaryTable.Rows[2].Cells[1].Paragraphs[0].Append(total.ToString("N2") + " RSD");
            doc.InsertTable(summaryTable);
            doc.InsertParagraph(Environment.NewLine);
            doc.InsertParagraph("-Sve cene su izražene u valuti: RSD").Alignment = Alignment.left;
            doc.InsertParagraph("-PR Vaš upravnik nije u sistemu PDV-a").Alignment = Alignment.left;
            doc.InsertParagraph("-Račun je validan bez pečata i potpisa").Alignment = Alignment.left;
            doc.InsertParagraph("-U pozivu na broj navedite broj računa").Alignment = Alignment.left;
        }


    }
}
