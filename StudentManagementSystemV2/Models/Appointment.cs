// Models/Appointment.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public int AppointmentId { get; set; }

        // Patient Information
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Student Number must be 8 digits")]
        public string StudentNumber { get; set; }

        // Appointment Details
        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime AppointmentTime { get; set; }

        // Nurse Assignment
        [ForeignKey("Nurse")]
        public int? NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }

        // Visit Information
        [Required]
        [Display(Name = "Reason for Visit")]
        public string Reason { get; set; }

        public List<SelectListItem> ReasonOptions { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "General Checkup", Text = "General Checkup" },
        new SelectListItem { Value = "Women's Health", Text = "Women's Health" },
        new SelectListItem { Value = "Men's Health", Text = "Men's Health" },
        new SelectListItem { Value = "Vaccination", Text = "Vaccination" },
        new SelectListItem { Value = "Other", Text = "Other" }
    };
        public string AdditionalInfo { get; set; }

        // Status Tracking
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Consent
        public bool ConfirmedDetails { get; set; }
        public bool ConsentForTreatment { get; set; }
        public DateTime? CheckInTime { get; set; }  // When patient checked in
        public string CancellationReason { get; set; } // Why appointment was cancelled
        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<Referral> Referrals { get; set; }
    }
}