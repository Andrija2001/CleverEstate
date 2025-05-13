namespace CleverEstate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Invoices", new[] { "Client_Id" });
            RenameColumn(table: "dbo.Invoices", name: "Client_Id", newName: "ClientId");
            AlterColumn("dbo.Invoices", "ClientId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Invoices", "ClientId");
            AddForeignKey("dbo.Invoices", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            AlterColumn("dbo.Invoices", "ClientId", c => c.Guid());
            RenameColumn(table: "dbo.Invoices", name: "ClientId", newName: "Client_Id");
            CreateIndex("dbo.Invoices", "Client_Id");
            AddForeignKey("dbo.Invoices", "Client_Id", "dbo.Clients", "Id");
        }
    }
}
