using CleverEstate.Models;
using CleverEstate.Services.Classes;
using System.Drawing;
using System.Windows.Forms;
using System;

namespace CleverEstate.Forms.Buildings
{
    public partial class FrmBuildings : Form
    {
        private  BuildingRepository repository;
        public  BindingSource bindingSource1 = new BindingSource();
        private  Button addNewRowButton = new Button();
        private  Button exitButton = new Button();
        private  Font font = new Font("Times New Roman", 14);
        private  Label titleLabel = new Label();
        private  Panel topPanel = new Panel();
        public FrmBuildings()
        {
            InitializeComponent();
            repository = new BuildingRepository(new DataDbContext());
            Load += FrmBuildings_Load;
            Resize += FrmBuildings_Resize;
            dataGridView1.Font = font;
        }

        private void FrmBuildings_Load(object sender, EventArgs e)
        {
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
   
  
     
        private void AdjustLayout()
        {
            addNewRowButton.Location = new Point(this.ClientSize.Width - 120, 10);
            exitButton.Location = new Point(this.ClientSize.Width - 120, this.ClientSize.Height - 60); 
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
                    Text = "Izmeni",
                    UseColumnTextForButtonValue = true,
                };
                dataGridView1.Columns.Add(btnEdit);
            }
            if (!dataGridView1.Columns.Contains("Delete"))
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Obriši",
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
            if (dataGridView1.Columns.Count > 3)
            {
                dataGridView1.Columns["Delete"].HeaderText = "Obrisi";
                dataGridView1.Columns["Edit"].HeaderText = "Izmeni";
                dataGridView1.Columns["Address"].HeaderText = "Adresa";
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
