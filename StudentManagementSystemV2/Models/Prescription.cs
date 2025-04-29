using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }// Pulled from appointment
        public string StudentNumber { get; set; }       // Pulled from appointment

        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Notes { get; set; }
        public int AppointmentId { get; set; }  // Add this property if it's missing
        public string Medication { get; set; }

        public int NurseId { get; set; }                // To track which nurse issued it
        public DateTime DateIssued { get; set; } = DateTime.Now;
    }
}