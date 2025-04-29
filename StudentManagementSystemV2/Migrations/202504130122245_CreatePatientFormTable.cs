namespace StudentManagementSystemV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePatientFormTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        StudentNumber = c.String(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        AppointmentTime = c.DateTime(nullable: false),
                        PreferredProvider = c.String(),
                        Reason = c.String(nullable: false),
                        AdditionalInfo = c.String(),
                        ConfirmedDetails = c.Boolean(nullable: false),
                        ConsentForTreatment = c.Boolean(nullable: false),
                        Status = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PatientForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Allergies = c.String(),
                        CurrentMedications = c.String(),
                        PreExistingConditions = c.String(),
                        HasDisability = c.Boolean(nullable: false),
                        DisabilityDetails = c.String(),
                        EmergencyContactName = c.String(nullable: false),
                        EmergencyContactRelationship = c.String(nullable: false),
                        EmergencyContactPhone = c.String(nullable: false),
                        EmergencyContactEmail = c.String(),
                        ConsentForTreatment = c.Boolean(nullable: false),
                        ConfidentialityAgreement = c.Boolean(nullable: false),
                        CommunicationMethod = c.String(nullable: false),
                        ConsentForStudentCare = c.Boolean(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        IsCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "HasCompletedPatientForm", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientForms", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PatientForms", new[] { "UserId" });
            DropIndex("dbo.Appointments", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "HasCompletedPatientForm");
            DropTable("dbo.PatientForms");
            DropTable("dbo.Appointments");
        }
    }
}
