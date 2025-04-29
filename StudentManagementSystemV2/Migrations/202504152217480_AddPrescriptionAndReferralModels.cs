namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrescriptionAndReferralModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        StudentNumber = c.String(),
                        MedicationName = c.String(),
                        Dosage = c.String(),
                        Frequency = c.String(),
                        Duration = c.String(),
                        Notes = c.String(),
                        NurseId = c.Int(nullable: false),
                        DateIssued = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Referrals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        StudentNumber = c.String(),
                        ReasonForReferral = c.String(),
                        MedicalHistory = c.String(),
                        AdditionalNotes = c.String(),
                        NurseId = c.Int(nullable: false),
                        DateReferred = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Referrals");
            DropTable("dbo.Prescriptions");
        }
    }
}
