namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedDateToDiagnosis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Diagnosis", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Diagnosis", "CreatedDate");
        }
    }
}
