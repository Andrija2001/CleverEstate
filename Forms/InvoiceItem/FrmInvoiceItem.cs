using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace CleverEstate.Forms.InvoiceItems
{
    public partial class FrmInvoiceItem : Form
    {
        private InvoiceItemService service;
        private List<InvoiceItem> datasource;
        public List<InvoiceItem> DataSource
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
            foreach (var InvoiceItem in datasource)
            {
               AddRowToPanel(tableLayoutPanel1,InvoiceItem);
            }
        }
        public FrmInvoiceItem()
        {
            InitializeComponent();
            service = new InvoiceItemService();
            LoadInvoiceItems();
        }
        public void LoadInvoiceItems()
        {
            DataSource = service.GetAllInvoiceItems();
        }
        private void AddRowToPanel(TableLayoutPanel panel, InvoiceItem invoiceItem)
        {
            int rowIndex = panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            Label lblQuantity = new Label();
            lblQuantity.DataBindings.Add("Text", invoiceItem, "Quantity");
            panel.Controls.Add(lblQuantity, 0, rowIndex);
            Label PricePerUnit = new Label();
            PricePerUnit.DataBindings.Add("Text", invoiceItem, "PricePerUnit");
            panel.Controls.Add(PricePerUnit, 1, rowIndex);
            Label lblVAT = new Label();
            lblVAT.DataBindings.Add("Text", invoiceItem, "VAT");
            panel.Controls.Add(lblVAT, 2, rowIndex);
            Label lblVATRate = new Label();
            lblVATRate.DataBindings.Add("Text", invoiceItem, "VATRate");
            panel.Controls.Add(lblVATRate, 3, rowIndex);
            Label lblTotalPrice = new Label();
            lblTotalPrice.DataBindings.Add("Text", invoiceItem, "TotalPrice");
            panel.Controls.Add(lblTotalPrice, 4, rowIndex);
            Label lblNumber = new Label();
            lblNumber.DataBindings.Add("Text", invoiceItem, "Number");
            panel.Controls.Add(lblNumber, 5, rowIndex);
            Button btnDelete = new Button() { Text = "Delete" };
            btnDelete.Click += (s, e) => {
                DeleteInvoiceItem(invoiceItem.Id);
            };
            panel.Controls.Add(btnDelete, 6, rowIndex);
            Button btnEdit = new Button() { Text = "Edit" };
            btnEdit.Click += (s, e) => {
                EditRow(invoiceItem.Id);
            };
            panel.Controls.Add(btnEdit, 7, rowIndex);
        }
        private void DeleteInvoiceItem(Guid id)
        {
            tableLayoutPanel1.SuspendLayout();
            service.Delete(id);
            LoadInvoiceItems();
            tableLayoutPanel1.ResumeLayout();
        }
        private void EditRow(Guid InvoiceItemId)
        {
            var InvoiceItem = service.GetAllInvoiceItems().FirstOrDefault(x => x.Id == InvoiceItemId);
            if (InvoiceItem != null)
            {
                FrmAddInvoiceItem frmedit = new FrmAddInvoiceItem(this, service, InvoiceItem);
                frmedit.ShowDialog();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAddInvoiceItem frmAddInvoiceItem = new FrmAddInvoiceItem(this,service);
            frmAddInvoiceItem.ShowDialog();
        }
    }
}