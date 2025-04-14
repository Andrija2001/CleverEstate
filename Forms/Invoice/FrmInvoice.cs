using CleverEstate.Models;
using CleverState.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CleverEstate.Forms.Invoices
{
    public partial class FrmInvoice : Form
    {
        public InvoiceService service;
        private List<Invoice> _dataSource;
        public List<Invoice> DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                DataBind();
            }
        }
        public  void DataBind()
        {
            tableLayoutPanel1.SuspendLayout();
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
            if (_dataSource == null)
                return;
            foreach (var invoice in _dataSource)
            {
                AddRowToPanel(tableLayoutPanel1, invoice);
            }
            tableLayoutPanel1.ResumeLayout();
        }
        public FrmInvoice()
        {
            InitializeComponent();
            service = new InvoiceService();
            LoadInvoices();
        }
        public void LoadInvoices()
        {
            DataSource = service.GetAllInvoices();
        }
        private void AddRowToPanel(TableLayoutPanel panel, Invoice invoice)
        {
            int rowIndex = panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            Label lblDate = new Label();
            lblDate.DataBindings.Add("Text", invoice, "Date");
            panel.Controls.Add(lblDate, 0, rowIndex);
            Label lblMonth = new Label();
            lblMonth.DataBindings.Add("Text", invoice, "Month");
            panel.Controls.Add(lblMonth, 1, rowIndex);
            Label lblPaymentDeadline = new Label();
            lblPaymentDeadline.DataBindings.Add("Text", invoice, "PaymentDeadline");
            panel.Controls.Add(lblPaymentDeadline, 2, rowIndex);
            Label lblPeriod = new Label();
            lblPeriod.DataBindings.Add("Text", invoice, "Period");
            lblPeriod.Dock = DockStyle.Fill;
            panel.Controls.Add(lblPeriod, 3, rowIndex);
            Label lblInvoiceNumber = new Label();
            lblInvoiceNumber.DataBindings.Add("Text", invoice, "InvoiceNumber");
            panel.Controls.Add(lblInvoiceNumber, 4, rowIndex);
            Label lblInvoiceDate = new Label();
            lblInvoiceDate.DataBindings.Add("Text", invoice, "InvoiceDate");
            panel.Controls.Add(lblInvoiceDate, 5, rowIndex);
            Label lblDescription = new Label();
            lblDescription.DataBindings.Add("Text", invoice, "Description");
            panel.Controls.Add(lblDescription, 6, rowIndex);
            Button btnDelete = new Button() { Text = "Delete" };
            btnDelete.Click += (s, e) => {
                DeleteApartment(invoice.Id);
            };
            panel.Controls.Add(btnDelete, 7, rowIndex);
            Button btnEdit = new Button() { Text = "Edit" };
            btnEdit.Click += (s, e) => {
                EditRow(invoice.Id);
            };
            panel.Controls.Add(btnEdit, 8, rowIndex);
        }
        private void DeleteApartment(Guid Id)
        {
            tableLayoutPanel1.SuspendLayout();
            service.Delete(Id);
            LoadInvoices();
            tableLayoutPanel1.ResumeLayout();
        }
        private void EditRow(Guid InvoiceId)
        {
            var Invoice = service.GetAllInvoices().FirstOrDefault(x => x.Id == InvoiceId);
            if (Invoice != null)
            {
                FrmAddInvoice frmedit = new FrmAddInvoice(this, service, Invoice);
                frmedit.ShowDialog();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAddInvoice frmAddInvoice = new FrmAddInvoice(this, service);
            frmAddInvoice.ShowDialog();
        
        }  
    }
}