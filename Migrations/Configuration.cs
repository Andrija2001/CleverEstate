﻿namespace CleverEstate.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CleverEstate.Models.DataDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }



    protected override void Seed(CleverEstate.Models.DataDbContext context)
        {
            
        }
    }
}
