namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStudentNumberValidation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StudentNumber", c => c.String());
            AddColumn("dbo.PatientForms", "StudentNumber", c => c.String(nullable: false, maxLength: 8));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientForms", "StudentNumber");
            DropColumn("dbo.AspNetUsers", "StudentNumber");
        }
    }
}
