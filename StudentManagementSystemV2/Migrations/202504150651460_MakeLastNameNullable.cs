namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeLastNameNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String());
        }

        public override void Down()
        {
            // Find all users with null LastName and set a default
            Sql("UPDATE dbo.AspNetUsers SET LastName = '' WHERE LastName IS NULL");
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
        }
    }
}
