namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMedicalHistoryRelationships : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Prescriptions", "AppointmentId");
            AddForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Prescriptions", new[] { "AppointmentId" });
        }
    }
}
