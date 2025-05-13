using CleverEstate.Models;
using CleverEstate.Services.Classes;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace CleverEstate.Forms.Buildings
{
    public partial class FrmBuildings : Form
    {
        private readonly BuildingRepository repository;
        public readonly BindingSource bindingSource1 = new BindingSource();
        private readonly Button addNewRowButton = new Button();
        private readonly Button exitButton = new Button();
        private readonly Font font = new Font("Segoe UI", 12);
        private readonly Label titleLabel = new Label();
        private readonly Panel topPanel = new Panel();
        public FrmBuildings()
        {
            InitializeComponent();
            repository = new BuildingRepository(new DataDbContext());
            InitializeDataGridView();
            SetupLayout();
            Load += FrmBuildings_Load;
            Resize += FrmBuildings_Resize; 
        }

        private void FrmBuildings_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadBuildingsFromRepository();
        }
        private void FrmBuildings_Resize(object sender, EventArgs e)
        {
            AdjustLayout();
        }
        private void LoadBuildingsFromRepository()
        {
            var buildingList = repository.GetAll();
            bindingSource1.DataSource = buildingList;
            dataGridView1.DataSource = bindingSource1;
        }
        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
        }
        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);
            dataGridView1.Location = new Point(10, 60); // Adjusted for better spacing
            dataGridView1.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 140); // Responsive to form size
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35); // Dark background
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // White text for contrast
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Gray;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 12);
        }
        private void SetupLayout()
        {
            this.Size = new Size(800, 600);
            this.Text = "Zgrade";
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 60;
            this.Controls.Add(topPanel);
            titleLabel.Text = "Zgrade";
            titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(20, 15); 
            topPanel.Controls.Add(titleLabel);
            addNewRowButton.Text = "Add Row";
            addNewRowButton.Font = font;
            addNewRowButton.Size = new Size(100, 40);
            addNewRowButton.Location = new Point(this.ClientSize.Width - 120, 10); // Positioned at top-right
            addNewRowButton.FlatStyle = FlatStyle.Flat;
            addNewRowButton.FlatAppearance.BorderSize = 0;
            addNewRowButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            addNewRowButton.Click += addNewRowButton_Click;
            topPanel.Controls.Add(addNewRowButton);
            exitButton.Text = "Exit";
            exitButton.Font = font;
            exitButton.Size = new Size(100, 50);
            exitButton.Location = new Point(this.ClientSize.Width - 120, this.ClientSize.Height - 60); // Positioned at bottom-right
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            exitButton.Click += exitButton_Click;
            this.Controls.Add(exitButton);
            AdjustLayout(); 
        }

        private void AdjustLayout()
        {
            addNewRowButton.Location = new Point(this.ClientSize.Width - 120, 10); // Right aligned
            exitButton.Location = new Point(this.ClientSize.Width - 120, this.ClientSize.Height - 60); // Right aligned at bottom
            dataGridView1.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 140);
        }

        private void addNewRowButton_Click(object sender, EventArgs e)
        {
            FrmAddBuildings frmAdd = new FrmAddBuildings(this, repository);
            if (frmAdd.ShowDialog() == DialogResult.OK)
            {
                LoadBuildingsFromRepository();
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
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
            if (dataGridView1.Columns.Contains("Id"))
            {
                dataGridView1.Columns["Id"].Visible = false;
            }
            if (dataGridView1.Columns.Count > 3)
            {
                dataGridView1.Columns["Delete"].DisplayIndex = 2;
                dataGridView1.Columns["Edit"].DisplayIndex = 3;
                dataGridView1.Columns["Address"].DisplayIndex = 0;
                dataGridView1.Columns["City"].DisplayIndex = 1;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
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
                    var selectedBuilding = (Building)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                    FrmAddBuildings frmEdit = new FrmAddBuildings(this, repository, selectedBuilding);
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        LoadBuildingsFromRepository();
                    }
                }
            }
        }
    }
}