using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class PrescriptionViewModel
    {
        public int AppointmentId { get; set; }

        [Required]
        public string MedicationName { get; set; }
     
        public string Medication { get; set; } // Add this if it's required in your controller
        public int NurseId { get; set; }
        [Required]
        public string Dosage { get; set; }

        [Required]
        public string Frequency { get; set; }

        [Required]
        public string Duration { get; set; }

        // Optionally include patient info for display
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
       
        public string StudentNumber { get; set; }
    }
}