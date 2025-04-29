namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStudentNumberValidationDigits : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PatientForms", "StudentNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PatientForms", "StudentNumber", c => c.String(nullable: false, maxLength: 8));
        }
    }
}
