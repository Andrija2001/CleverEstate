namespace CleverEstate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Buildings", "City", c => c.String());
            DropColumn("dbo.Clients", "City");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "City", c => c.String());
            DropColumn("dbo.Buildings", "City");
        }
    }
}
