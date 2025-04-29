using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystemV2.Models
{
    public class Nurse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nurse Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } // Used for login

        [Required]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; } // "General", "Women", "Men"

        [Display(Name = "Max Daily Appointments")]
        public int MaxDailyAppointments { get; set; } = 12;

        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        // Navigation property
        public virtual ICollection<Appointment> Appointments { get; set; }
        // Add these navigation properties:
        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
        public virtual ICollection<Referral> Referrals { get; set; }
    }
}
