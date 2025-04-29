namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDiagnosisModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Diagnosis", "LastName", c => c.String());
            AddColumn("dbo.Diagnosis", "FirstName", c => c.String());
            DropColumn("dbo.Diagnosis", "PatientName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Diagnosis", "PatientName", c => c.String());
            DropColumn("dbo.Diagnosis", "FirstName");
            DropColumn("dbo.Diagnosis", "LastName");
        }
    }
}
