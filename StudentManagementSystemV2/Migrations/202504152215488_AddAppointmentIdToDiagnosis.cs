namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppointmentIdToDiagnosis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Diagnosis", "AppointmentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Diagnosis", "AppointmentId");
        }
    }
}
