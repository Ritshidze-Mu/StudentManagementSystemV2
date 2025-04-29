namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMedicationAndAppointmentIdToPrescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "AppointmentId", c => c.Int(nullable: false));
            AddColumn("dbo.Prescriptions", "Medication", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescriptions", "Medication");
            DropColumn("dbo.Prescriptions", "AppointmentId");
        }
    }
}
