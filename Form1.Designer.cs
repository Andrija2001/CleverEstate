namespace CleverEstate
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.glavniMeniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ApartmantsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemCatalogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.glavniMeniToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // glavniMeniToolStripMenuItem
            // 
            this.glavniMeniToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BuildingsToolStripMenuItem,
            this.ApartmantsToolStripMenuItem,
            this.InvoiceToolStripMenuItem,
            this.ItemCatalogToolStripMenuItem});
            this.glavniMeniToolStripMenuItem.Name = "glavniMeniToolStripMenuItem";
            this.glavniMeniToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.glavniMeniToolStripMenuItem.Text = "Glavni Meni";
            // 
            // BuildingsToolStripMenuItem
            // 
            this.BuildingsToolStripMenuItem.Name = "BuildingsToolStripMenuItem";
            this.BuildingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.BuildingsToolStripMenuItem.Text = "Buildings";
            this.BuildingsToolStripMenuItem.Click += new System.EventHandler(this.BuildingToolStripMenuItem_Click);
            // 
            // ApartmantsToolStripMenuItem
            // 
            this.ApartmantsToolStripMenuItem.Name = "ApartmantsToolStripMenuItem";
            this.ApartmantsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ApartmantsToolStripMenuItem.Text = "Apartmans";
            this.ApartmantsToolStripMenuItem.Click += new System.EventHandler(this.ApartmantsToolStripMenuItem_Click);
            // 
            // InvoiceToolStripMenuItem
            // 
            this.InvoiceToolStripMenuItem.Name = "InvoiceToolStripMenuItem";
            this.InvoiceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.InvoiceToolStripMenuItem.Text = "Invoice";
            this.InvoiceToolStripMenuItem.Click += new System.EventHandler(this.InvoiceToolStripMenuItem_Click);
            // 
            // ItemCatalogToolStripMenuItem
            // 
            this.ItemCatalogToolStripMenuItem.Name = "ItemCatalogToolStripMenuItem";
            this.ItemCatalogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ItemCatalogToolStripMenuItem.Text = "Item Catalog";
            this.ItemCatalogToolStripMenuItem.Click += new System.EventHandler(this.ItemCatalogToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(220, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem glavniMeniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BuildingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ApartmantsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InvoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ItemCatalogToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}

