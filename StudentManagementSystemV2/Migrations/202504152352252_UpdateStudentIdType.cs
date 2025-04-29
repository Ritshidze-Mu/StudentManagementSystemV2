namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStudentIdType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Students", new[] { "UserId" });

            // ✅ Drop the primary key first
            DropPrimaryKey("dbo.Students");

            // ✅ Now it's safe to drop the column
            DropColumn("dbo.Students", "StudentId");

            // ✅ Rename 'UserId' to 'StudentId'
            RenameColumn(table: "dbo.Students", name: "UserId", newName: "StudentId");

            // ✅ Update the column type (make it string, max length 128)
            AlterColumn("dbo.Students", "StudentId", c => c.String(nullable: false, maxLength: 128));

            // ✅ Re-add the primary key on the new 'StudentId'
            AddPrimaryKey("dbo.Students", "StudentId");

            // New columns
            AddColumn("dbo.Referrals", "AppointmentId", c => c.Int(nullable: false));
            AddColumn("dbo.Referrals", "ReferredToDepartment", c => c.String());

            // Indexes
            CreateIndex("dbo.Students", "StudentId");
            CreateIndex("dbo.Diagnosis", "AppointmentId");
            CreateIndex("dbo.Diagnosis", "NurseId");
            CreateIndex("dbo.Referrals", "AppointmentId");
            CreateIndex("dbo.Referrals", "NurseId");

            // Foreign keys
            AddForeignKey("dbo.Diagnosis", "AppointmentId", "dbo.Appointments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Diagnosis", "NurseId", "dbo.Nurses", "Id");
            AddForeignKey("dbo.Referrals", "AppointmentId", "dbo.Appointments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Referrals", "NurseId", "dbo.Nurses", "Id");
        }


        public override void Down()
        {
            DropForeignKey("dbo.Referrals", "NurseId", "dbo.Nurses");
            DropForeignKey("dbo.Referrals", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Diagnosis", "NurseId", "dbo.Nurses");
            DropForeignKey("dbo.Diagnosis", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Referrals", new[] { "NurseId" });
            DropIndex("dbo.Referrals", new[] { "AppointmentId" });
            DropIndex("dbo.Diagnosis", new[] { "NurseId" });
            DropIndex("dbo.Diagnosis", new[] { "AppointmentId" });
            DropIndex("dbo.Students", new[] { "StudentId" });
            DropPrimaryKey("dbo.Students");
            AlterColumn("dbo.Students", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Students", "StudentId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Referrals", "ReferredToDepartment");
            DropColumn("dbo.Referrals", "AppointmentId");
            AddPrimaryKey("dbo.Students", "StudentId");
            RenameColumn(table: "dbo.Students", name: "StudentId", newName: "UserId");
            AddColumn("dbo.Students", "StudentId", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Students", "UserId");
        }
    }
}
