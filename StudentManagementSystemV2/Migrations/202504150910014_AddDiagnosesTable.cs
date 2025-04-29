namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiagnosesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Diagnosis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientName = c.String(),
                        StudentNumber = c.String(),
                        BloodPressure = c.String(),
                        Temperature = c.String(),
                        Pulse = c.String(),
                        RespiratoryRate = c.String(),
                        ChiefComplaint = c.String(),
                        NursesAssessment = c.String(),
                        DiagnosisDescription = c.String(),
                        Plan = c.String(),
                        NurseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Diagnosis");
        }
    }
}
