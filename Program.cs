using CleverEstate.Forms;
using CleverEstate.Forms.Apartments;
using CleverEstate.Forms.Buildings;
using CleverEstate.Forms.CatalogItem;
using CleverEstate.Forms.Clients;
using CleverEstate.Forms.InvoiceItems;
using CleverEstate.Forms.Invoices;
using CleverEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CleverEstate
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
