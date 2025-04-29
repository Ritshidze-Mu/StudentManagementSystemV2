namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppointmentStatusFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "CheckInTime", c => c.DateTime());
            AddColumn("dbo.Appointments", "CancellationReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "CancellationReason");
            DropColumn("dbo.Appointments", "CheckInTime");
        }
    }
}
