using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using StudentManagementSystemV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagementSystemV2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // Link to Student (one-to-one)
        public virtual Student Student { get; set; }

        // Navigation properties for other models
        public virtual ICollection<Appointment> Appointments { get; set; }

        // Additional properties for the application user
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; } // Student number
        public string FullName { get; set; }
        public string Address { get; set; }
        public string IDNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Account tracking
        public bool IsActive { get; set; } = true;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }
        public bool HasCompletedPatientForm { get; set; }

        // Required identity method
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<PatientForm> PatientForms { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Referral> Referrals { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure that UserId is the PK for Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.UserId);  // Set UserId as the primary key for Student

    // Configure the one-to-one relationship between Student and ApplicationUser
    modelBuilder.Entity<Student>()
        .HasRequired(s => s.User)  // Student requires a User (one-to-one relationship)
        .WithOptional(u => u.Student)  // ApplicationUser optionally has a Student
        .WillCascadeOnDelete(true);  // Enable cascade delete (when a User is deleted, the associated Student will be deleted)

            // Other configurations for relationships
            modelBuilder.Entity<Diagnosis>()
                .HasRequired(d => d.Nurse)
                .WithMany(n => n.Diagnoses)
                .HasForeignKey(d => d.NurseId)
                .WillCascadeOnDelete(false);  // No cascade delete for Diagnosis -> Nurse relationship

            modelBuilder.Entity<Referral>()
                .HasRequired(r => r.Nurse)
                .WithMany(n => n.Referrals)
                .HasForeignKey(r => r.NurseId)
                .WillCascadeOnDelete(false);  // No cascade delete for Referral -> Nurse relationship

            // Ignore SelectListItem from the model (if it's used in your views but shouldn't be in the model)
            modelBuilder.Ignore<System.Web.Mvc.SelectListItem>();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}