namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNurseAndUpdateAppointmentModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Nurses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Specialization = c.String(nullable: false),
                        MaxDailyAppointments = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Appointments", "NurseId", c => c.Int());
            CreateIndex("dbo.Appointments", "NurseId");
            AddForeignKey("dbo.Appointments", "NurseId", "dbo.Nurses", "Id");
            DropColumn("dbo.Appointments", "PreferredProvider");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "PreferredProvider", c => c.String());
            DropForeignKey("dbo.Appointments", "NurseId", "dbo.Nurses");
            DropIndex("dbo.Appointments", new[] { "NurseId" });
            DropColumn("dbo.Appointments", "NurseId");
            DropTable("dbo.Nurses");
        }
    }
}
