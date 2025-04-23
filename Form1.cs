﻿using CleverEstate.Forms.Apartments;
using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.CatalogItem;
using CleverEstate.Forms.Clients;
using CleverEstate.Forms.InvoiceItems;
using CleverEstate.Forms.Invoices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleverEstate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BuildingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBuildings frmBuildings = new FrmBuildings();
            frmBuildings.ShowDialog();
        }

        private void ApartmantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmApartment frmApartment = new FrmApartment();
            frmApartment.ShowDialog();
        }

        private void InvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInvoice frmInvoice = new FrmInvoice();
            frmInvoice.ShowDialog();
        }

        private void ItemCatalogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmItemCatalog frmItemCatalog = new FrmItemCatalog();
            frmItemCatalog.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmInvoiceItem frmInvoiceItem = new FrmInvoiceItem();
            frmInvoiceItem.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           FrmClient frmClient = new FrmClient();
            frmClient.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmInvoiceItem frmInvoiceItem = new FrmInvoiceItem();
            frmInvoiceItem.ShowDialog();
        }
    }
}
