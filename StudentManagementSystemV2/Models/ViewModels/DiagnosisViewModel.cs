using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class DiagnosisViewModel
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string StudentNumber { get; set; }

        [Required]
        public string BloodPressure { get; set; }

        [Required]
        public string Temperature { get; set; }

        [Required]
        public string Pulse { get; set; }

        [Required]
        public string RespiratoryRate { get; set; }

        public string ChiefComplaint { get; set; }
        public string NursesAssessment { get; set; }

        [Display(Name = "Diagnosis")]
        public string DiagnosisDescription { get; set; }

        public string Plan { get; set; }

        public int NurseId { get; set; }
       
    }
}