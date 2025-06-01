using CleverEstate.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using CleverEstate.Services.Classes.Repository;
using CleverEstate.Services.Interface.Repository;
using CleverEstate.Services.Classes;
using System.Collections.Generic;
using System.Linq;
using CleverEstate.Forms.Buildings;
using System.Net.NetworkInformation;

namespace CleverEstate.Forms.Apartments
{
    public partial class FrmApartment : Form
    {
        private  ApartmentRepository repository;
        private  BuildingRepository buildingRepository;
        private  ClientRepository clientRepository;
        public  BindingSource bindingSource1 = new BindingSource();
        private  Button addNewRowButton = new Button();
        private  Button exitButton = new Button();
        private  Font font = new Font("Times New Roman", 14);
        private  Label titleLabel = new Label();
        private  Label lblZgrade = new Label();
        private  ComboBox comboBoxAdd = new ComboBox();
        private  Button btnAdd = new Button();
        private  Panel topPanel = new Panel();
        public FrmApartment()
        {
            InitializeComponent();
            repository = new ApartmentRepository(new DataDbContext());
            clientRepository = new ClientRepository(new DataDbContext());
            buildingRepository = new BuildingRepository(new DataDbContext());
            InitializeDataGridView();
            dataGridView1.CellClick += dataGridView1_CellClick;
            SetupLayout();
        }
        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }
        private void FrmApartment_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadApartmants();
            FillComboBox();
        }
        private void FillComboBox()
        {
            var buildings = buildingRepository.GetAll();
            comboBoxAdd.DataSource = buildings;
            comboBoxAdd.DisplayMember = "Address";
            comboBoxAdd.ValueMember = "Id";
            if (comboBoxAdd.Items.Count > 0)
            {
                comboBoxAdd.SelectedIndex = 0;
            }
        }
        public void LoadApartmants()
        {
            var clients = clientRepository.GetAll();
            var apartmants = repository.GetAll();
            var combinedData = apartmants.Select(a => new
            {
                ApartmanId = a.Id,
                BrojApartmana = a.Number,
                Površina = a.Area,
                KlijentId = a.ClientId,
                ImeKlijenta = clients.FirstOrDefault(k => k.Id == a.ClientId)?.Name ?? "N/A",
                PrezimeKlijenta = clients.FirstOrDefault(k => k.Id == a.ClientId)?.Surname ?? "N/A",
                Id = a.Id 
            }).ToList();
            dataGridView1.DataSource = combinedData;
        }
        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);
            dataGridView1.Location = new Point(10, 100);
            dataGridView1.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 140);
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
           
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Gray;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.Font = font;
         

        }
        private void SetupLayout()
        {
            this.Size = new Size(800, 600);
            this.Text = "Stanovi";
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 100;
            this.Controls.Add(topPanel);
            titleLabel.Text = "Stanovi";
            titleLabel.Font = new Font("Times New Roman", 14);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(20, 15);
            topPanel.Controls.Add(titleLabel);
            addNewRowButton.Text = "Add Row";
            addNewRowButton.Font = font;
            addNewRowButton.Size = new Size(100, 40);
            addNewRowButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            addNewRowButton.Location = new Point(this.ClientSize.Width - 120, 55);
            addNewRowButton.FlatStyle = FlatStyle.Flat;
            addNewRowButton.FlatAppearance.BorderSize = 0;
            addNewRowButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            addNewRowButton.Click += addNewRowButton_Click;
            topPanel.Controls.Add(addNewRowButton);
            lblZgrade.Text = "Adresa:";
            lblZgrade.Font = font;
            lblZgrade.AutoSize = true;
            lblZgrade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblZgrade.Location = new Point(this.ClientSize.Width - 250, 22);
            topPanel.Controls.Add(lblZgrade);
            comboBoxAdd.Location = new Point(this.ClientSize.Width - 180, 20);
            comboBoxAdd.Size = new Size(120, 30);
            comboBoxAdd.Font = font;
            comboBoxAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBoxAdd.SelectedIndexChanged += AddressFilter;
            topPanel.Controls.Add(comboBoxAdd);
            exitButton.Text = "Izadji";
            exitButton.Font = font;
            exitButton.Size = new Size(100, 30);
            exitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            exitButton.Location = new Point(this.ClientSize.Width - 100, this.ClientSize.Height - 20);
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.FlatAppearance.BorderSize = 0;          
            this.Controls.Add(exitButton);
            this.Resize += FrmApartment_Resize;
        }
        private void FrmApartment_Resize(object sender, EventArgs e)
        {
            addNewRowButton.Location = new Point(topPanel.Width - addNewRowButton.Width - 20, 55);
            lblZgrade.Location = new Point(topPanel.Width - 280, 30);
            comboBoxAdd.Location = new Point(topPanel.Width - 210, 32);
            exitButton.Location = new Point(this.ClientSize.Width - exitButton.Width - 10, this.ClientSize.Height - exitButton.Height - 10);
            dataGridView1.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - topPanel.Height - exitButton.Height - 30);
        }
        private void AddressFilter(object sender, EventArgs e)
        {
            FiltrirajAdrese();
        }
        public void FiltrirajAdrese()
        {
            string adresa = comboBoxAdd.Text;
            var zgrade = buildingRepository.GetAll();
            var apartmani = repository.GetAll();
            var klijenti = clientRepository.GetAll();
            var building = zgrade.FirstOrDefault(b => b.Address == adresa);
            if (building == null)
            {
                return;
            }
            var apartmanizazgradu = apartmani.Where(a => a.BuildingId == building.Id).ToList();
            var prikaz = apartmanizazgradu.Select(a =>
            {
                var klijent = klijenti.FirstOrDefault(k => k.Id == a.ClientId);
                return new
                {
                    a.Number,
                    a.Area,
                    Name = klijent?.Name ?? "Nepoznato",
                    Surname = klijent?.Surname ?? "Nepoznato",
                    a.Id,
                    a.ClientId,
                };
            }).ToList();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = prikaz;
            if (dataGridView1.Columns.Contains("Id"))
            {
                dataGridView1.Columns["Id"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("BuildingId"))
            {
                dataGridView1.Columns["BuildingId"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("ClientId"))
            {
                dataGridView1.Columns["ClientId"].Visible = false;
            }
            if (dataGridView1.Columns.Contains("ClientId"))
            {
                dataGridView1.Columns["Area"].HeaderText = "Površina";
                dataGridView1.Columns["Name"].HeaderText = "Ime";
                dataGridView1.Columns["Surname"].HeaderText = "Prezime";
                dataGridView1.Columns["Edit"].HeaderText = "Izmeni";
                dataGridView1.Columns["Delete"].HeaderText = "Obriši";
            }
        }
        private void addNewRowButton_Click(object sender, EventArgs e)
        {
            var selectedBuildingId = comboBoxAdd.SelectedValue as Guid?;

            if (selectedBuildingId.HasValue)
            {
                FrmAddApartment frmAddApartment = new FrmAddApartment(this, repository, selectedBuildingId.Value, clientRepository);
                frmAddApartment.ShowDialog();
            }
            else
            {
                MessageBox.Show("Molimo izaberite adresu.", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (!dataGridView1.Columns.Contains("Edit"))
            {
                var btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                };
                dataGridView1.Columns.Add(btnEdit);
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

            if (dataGridView1.Columns.Count > 3)
            {
                dataGridView1.Columns["Delete"].DisplayIndex = 4;
                dataGridView1.Columns["Edit"].DisplayIndex = 5;
               
            }
            string[] idColumns = { "Id", "BuildingId", "ClientId" };
            foreach (string colName in idColumns)
            {
                if (dataGridView1.Columns.Contains(colName))
                {
                    dataGridView1.Columns[colName].Visible = false;
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                    var buildingToDelete = (Guid)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
                    repository.Delete(buildingToDelete); 
                    BindingSource bindingSource = (BindingSource)dataGridView1.DataSource;
                    bindingSource.RemoveAt(e.RowIndex);
                    dataGridView1.Refresh();
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                var selectedClientId = (Guid)dataGridView1.Rows[e.RowIndex].Cells["ClientId"].Value;
                var selectedApartmentId = (Guid)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;       
                var selectedBuilding = (Guid)comboBoxAdd.SelectedValue;
                var selectedApartment = repository.GetById(selectedApartmentId);
                var currentClient = clientRepository.GetById(selectedClientId);
                FrmAddApartment frmEdit = new FrmAddApartment(this, repository, selectedApartment, selectedBuilding, clientRepository, currentClient);
                if (frmEdit.ShowDialog() == DialogResult.OK)
                {
                    FiltrirajAdrese();
                }
            }
        }
    }
}
