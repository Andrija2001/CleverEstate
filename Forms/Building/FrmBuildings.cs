using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate.Forms.Buildings
{
    public partial class FrmBuildings : Form
    {
        private BuildingService service;
        private List<Building> datasource;
        public List<Building> DataSource
        {
            get => datasource;
            set
            {
                datasource = value;
                DataBind();
            }
        }
        private void DataBind()
        {
            for (int i = tableLayoutPanel1.RowCount - 1; i > 0; i--)
            {
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(j, i);
                    if (control != null)
                        tableLayoutPanel1.Controls.Remove(control);
                }
            }
            while (tableLayoutPanel1.RowStyles.Count > 1)
            {
                tableLayoutPanel1.RowStyles.RemoveAt(1);
            }
            tableLayoutPanel1.RowCount = 1;
            if (datasource == null)
                return;
            var existingBuilding = new HashSet<string>();
            foreach (var building in datasource)
            {
                string Key = building.Address;
                if (existingBuilding.Contains(Key))
                {
                    MessageBox.Show("Zgrada sa ovom adresom već postoji");
                    return;
                }
                existingBuilding.Add(Key);
                AddRowToPanel(tableLayoutPanel1, building);
            }
        }
        public FrmBuildings()
        {
            InitializeComponent();
            service = new BuildingService();
            LoadBuildings();
        }
        public void LoadBuildings()
        {
            DataSource = service.GetAllBuildings();
        }
        private void AddRowToPanel(TableLayoutPanel panel, Building building)
        {
            int rowIndex = panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            Label lbladdress = new Label();
            lbladdress.DataBindings.Add("Text", building, "Address");
            panel.Controls.Add(lbladdress, 0, rowIndex);
            Button btnDelete = new Button() { Text = "Delete" };
            btnDelete.Click += (s, e) => {
                DeleteBuilding(building.Id);
            };
            panel.Controls.Add(btnDelete, 1, rowIndex);
            Button btnEdit = new Button() { Text = "Edit" };
            btnEdit.Click += (s, e) => {
                EditRow(building.Id);
            };
            panel.Controls.Add(btnEdit, 2, rowIndex);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAddBuildings frmAddBuildings = new FrmAddBuildings(this, service);
            frmAddBuildings.ShowDialog();
        }
        private void EditRow(Guid Id)
        {
            var building = service.GetAllBuildings().FirstOrDefault(x => x.Id == Id);
            if (building != null)
            {
                FrmAddBuildings frmedit = new FrmAddBuildings(this, service, building);
                frmedit.ShowDialog();
            }
        }
        private void DeleteBuilding(Guid Id)
        {
            tableLayoutPanel1.SuspendLayout();
            service.Delete(Id);
            LoadBuildings();
            tableLayoutPanel1.ResumeLayout(); 
        }
    }
}