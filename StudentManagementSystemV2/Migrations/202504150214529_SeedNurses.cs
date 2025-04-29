namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedNurses : DbMigration
    {
        public override void Up()
        {
            // Add the Email column (you may already have this in your model)
            AddColumn("dbo.Nurses", "Email", c => c.String(nullable: false));

            // You do NOT need to add a Password column here since it will be handled by ASP.NET Identity
        }

        public override void Down()
        {
            // Rollback the migration, removing the Email column
            DropColumn("dbo.Nurses", "Email");

            // Don't remove the Password column since it won't be added directly here.
        }
    }
}
