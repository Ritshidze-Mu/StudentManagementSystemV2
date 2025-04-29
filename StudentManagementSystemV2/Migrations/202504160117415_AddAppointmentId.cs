namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppointmentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "AppointmentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "AppointmentId");
        }
    }
}
